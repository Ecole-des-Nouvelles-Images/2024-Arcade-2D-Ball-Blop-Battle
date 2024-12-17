using System;
using System.Collections.Generic;
using Hugo.Prototype.Scripts.Arene;
using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Camera;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Special Spike
        public int CountShootSpecialSpike;
        public bool WinTheMatch;
        public bool LoseTheMatch;
        
        // Components
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        private PlayerNumberTouchBallHandler _playerNumberTouchBallHandler;
        private Animator _animator;
        private CameraHandler _cameraHandler;
        private AudioSource _audioSource;
        
        // GameObject
        private GameObject _ball;
        private GameObject _arenaVFXHandler;
        
        // States
        private bool _hasTheBall;
        private bool _isDashing;
        private bool _canPerfectReception;
        private bool _canMove = true;
        private bool _isGrounded;
        private bool _isWalled;
        private bool _canDoubleJump;
        public bool CanAbsorb;
        private bool _isAttacking;
        private bool _appears;
        private bool _hasPunch;
        
        // Inputs values
        private Vector2 _move;
        private float _isWestButtonPressed;
        
        // Special spike
        public int PerfectReceptionCount;
        public bool CanSpecialSpike;
        public bool IsSpecialSpike;
        private bool _shootSpecialSpike;
        
        // Player Type
        [Header("Player Type")]
        public PlayerType PlayerType;
        
        // Player Settings
        [Header("Player Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _wallJumpForce;
        [SerializeField] private float _jumpingSpeed;
        [SerializeField] private float _airControlFactor;
        [SerializeField] private float _maxAirSpeed;
        [SerializeField] private float _timeCanPerfectReception;
        [SerializeField] private float _timeAppears;
        [SerializeField] private float _timeResetRotation;
        
        // Dash Settings
        [Header("Dash Settings")]
        [SerializeField] private float _dashSpeed;
        [SerializeField] private float _dashDuration;
        [SerializeField] private float _dashCooldown;
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;
        
        // isGrounded and isWalled
        [Header("Is Grounded and Is Walled")]
        [SerializeField] private float _rayGroundedLength;
        [SerializeField] private float _rayWalledLength;
        [SerializeField] private float _rayNetTouchedLength;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _wallLayer;
        
        [Header("BackGround SpecialSpike")]
        [SerializeField] private GameObject _impactBackground;
        [SerializeField] private GameObject _scrollBackground;
        public GameObject ImpactBackgroundObject;
        public static List<GameObject> ScrollBackgroundObjects = new List<GameObject>();
        
        // VFX
        [Header("   VFX Effects")]
        [Header("Common")]
        [SerializeField] private ParticleSystem _vfxJumping;
        [SerializeField] private ParticleSystem _vfxAttacking;
        [SerializeField] private ParticleSystem _vfxPerfectReception;
        [SerializeField] private ParticleSystem _vfxActiveSpecialSpike;
        [Header("SpitOut and Landing")]
        [SerializeField] private ParticleSystem _vfxSpitOutBlue;
        [SerializeField] private ParticleSystem _vfxLandingBlue;
        [SerializeField] private ParticleSystem _vfxSpitOutGreen;
        [SerializeField] private ParticleSystem _vfxLandingGreen;
        [SerializeField] private ParticleSystem _vfxSpitOutYellow;
        [SerializeField] private ParticleSystem _vfxLandingYellow;
        [SerializeField] private ParticleSystem _vfxSpitOutRed;
        [SerializeField] private ParticleSystem _vfxLandingRed;
        [Header("Shoot Special Spike")]
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeBlue;
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeGreen;
        [SerializeField] private ParticleSystem _vfxShootSpecialSpikeYellow;
        public ParticleSystem VfxShootSpecialSpikeRed;
        [Header("Trails")]
        [SerializeField] private GameObject _vfxTrailBlue;
        [SerializeField] private GameObject _vfxTrailGreen;
        [SerializeField] private GameObject _vfxTrailYellow;
        [SerializeField] private GameObject _vfxTrailRed;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _playerNumberTouchBallHandler = GetComponent<PlayerNumberTouchBallHandler>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            _arenaVFXHandler = GameObject.FindGameObjectWithTag("ArenaParticleSystem");
            _cameraHandler = GameObject.FindWithTag("MainCamera").GetComponent<CameraHandler>();
            PlayerType = _playerNumberTouchBallHandler.IsPlayerOne ? GameManager.FirstPlayerScriptableObject : GameManager.SecondPlayerScriptableObject;

            // VFX TRAILS
            if (_vfxTrailBlue && _vfxTrailGreen && _vfxTrailYellow && _vfxTrailRed)
            {
                if (PlayerType.PlayerName == "Bleu")
                {
                    _vfxTrailBlue.SetActive(true);
                }
                if (PlayerType.PlayerName == "Vert")
                {
                    _vfxTrailGreen.SetActive(true);
                }
                if (PlayerType.PlayerName == "Jaune")
                {
                    _vfxTrailYellow.SetActive(true);
                }
                if (PlayerType.PlayerName == "Rouge")
                {
                    _vfxTrailRed.SetActive(true);
                }
            }
        }

        private void Start()
        {
            _canMove = false;
            _appears = true;
            Invoke(nameof(ReverseCanMove), _timeAppears);
            Invoke(nameof(ReverseAppears), _timeAppears);

            if (PlayerType)
            {
                _sr.sprite = PlayerType.Sprite;
                _animator.runtimeAnimatorController = PlayerType.PlayerAnimatorController;
            }
        }

        private void Update()
        {
            Raycasts();
            Dash();
            
            _ball = GameObject.FindGameObjectWithTag("Ball");
            
            // Faute when _hasTheBall && _isGrounded
            if (_isGrounded && _hasTheBall && !IsSpecialSpike)
            {
                _playerNumberTouchBallHandler.Fouls();
                _hasTheBall = false;
            }
            
            // Reset States End of Timer
            if (MatchManager.IsSetOver)
            {
                _hasTheBall = false;
                IsSpecialSpike = false;
            }
            
            // Cant Perfect reception dans les airs
            if (!_isGrounded)
            {
                _canPerfectReception = false;
            }

            if (WinTheMatch && _isGrounded && _canMove)
            {
                // _canMove = false;
                _animator.SetTrigger("Win");
            }
            if (LoseTheMatch && _isGrounded && _canMove)
            {
                // _canMove = false;
                _animator.SetTrigger("Lose");
            }
            
            // Animation
            _animator.SetBool("Attack", _isAttacking);
            _animator.SetBool("HasTheBall", _hasTheBall);
            _animator.SetBool("IsWalled", _isWalled);
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("CanAbsorb", CanAbsorb);
            _animator.SetBool("PerfectReception", _canPerfectReception);
            _animator.SetBool("Dash", _isDashing);
            _animator.SetBool("Appears", _appears);
            _animator.SetFloat("GoUp", _rb2d.velocity.y);
            _animator.SetFloat("MaxJumpHeight", _rb2d.velocity.y);
            _animator.SetFloat("Falling", _rb2d.velocity.y);
        }

        private void FixedUpdate()
        {
            if (!_hasTheBall)
            {
                _sr.color = new Color(1, 1, 1, 0.9f);
                
                var horizontalInput = _move.x;

                if (_isGrounded)
                {
                    Vector2 movement = new Vector2(horizontalInput * (_speed * Time.deltaTime), _rb2d.velocity.y);
                    _rb2d.velocity = movement;
                }
                else
                {
                    float airSpeed = horizontalInput * _speed * _airControlFactor;
                    float newMovement = Math.Clamp(_rb2d.velocity.x + airSpeed * Time.fixedDeltaTime, -_maxAirSpeed, _maxAirSpeed);
                    _rb2d.velocity = new Vector2(newMovement, _rb2d.velocity.y);
                }
            }
            else
            {
                _sr.color = new Color(1, 1, 1, 0.7f);
            }
            
            // Animation
            _animator.SetFloat("Speed", Mathf.Abs(_rb2d.velocity.x));
            FlipSprite(_rb2d.velocity.x);
            // VFX
            // if (_vfxWalking)
            // {
            //     bool isWalking = _rb2d.velocity.x is > 0.3f or < -0.3f;
            //     ParticleSystem.EmissionModule emission = _vfxWalking.emission;
            //     emission.enabled = _isGrounded && isWalking;
            // }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                if (_canPerfectReception)
                {
                    _ball.GetComponent<BallHandler>().PerfectReception();
                    PerfectReceptionCount++;
                    
                    // VFX
                    if (_vfxPerfectReception)
                    {
                        _vfxPerfectReception.Play();
                    }
                    
                    if (PerfectReceptionCount == 3)
                    { 
                        CanSpecialSpike = true;
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
                
                Vector2 direction = new Vector2(_ball.transform.position.x - transform.position.x, _ball.transform.position.y - transform.position.y);
                
                if (Mathf.Approximately(_isWestButtonPressed, 1) && !_isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 2)
                {
                    // && _playerNumberTouchBallHandler.NumberTouchBall < 2
                    
                    _ball.GetComponent<BallHandler>().IsAbsorb(gameObject);
                    _hasTheBall = true;
                    _isDashing = false;
                    _canMove = true;
                    
                    FlipSpriteAbsorbDrawn(direction);
                    
                    // Animation
                    _animator.SetTrigger("Absorb");
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
                
                if(_isWestButtonPressed == 0 && _playerNumberTouchBallHandler.NumberTouchBall < 2 && !_hasPunch)
                {
                    _ball.GetComponent<BallHandler>().IsPunch(direction, _rb2d.velocity);

                    _isAttacking = true;
                    _hasPunch = true;
                    Invoke(nameof(ReverseIsAttacking), 0.2f);
                    Invoke(nameof(ReverseHasPunch), 0.1f);
                    
                    // Shake Camera
                    _cameraHandler.HitShake();
                    
                    // VFX
                    if (_vfxAttacking)
                    {
                        _vfxAttacking.Play();
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }

                if (IsSpecialSpike && _playerNumberTouchBallHandler.NumberTouchBall < 2 && !_isGrounded)
                {
                    _hasTheBall = true;
                    CanAbsorb = false;
                    _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    _ball.GetComponent<BallHandler>().IsAbsorb(gameObject);
                    
                    FlipSpriteAbsorbDrawn(direction);
                    
                    if (_scrollBackground)
                    {
                        if (PlayerType.PlayerName == "Bleu")
                        {
                            _scrollBackground.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                            Instantiate(_scrollBackground, new Vector2(0, -9), Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Vert")
                        {
                            _scrollBackground.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                            Instantiate(_scrollBackground, new Vector2(0, -9), Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Jaune")
                        {
                            _scrollBackground.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                            Instantiate(_scrollBackground, new Vector2(0, -9), Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Rouge")
                        {
                            _scrollBackground.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                            Instantiate(_scrollBackground, new Vector2(0, -9), Quaternion.identity);
                        }
                    }
                    Destroy(ImpactBackgroundObject);
                    
                    // Animation
                    _animator.SetTrigger("Absorb");
                    
                    // VFX
                    if (VfxShootSpecialSpikeRed)
                    {
                        if (PlayerType.PlayerName == "Rouge")
                        {
                            _ball.GetComponent<BallHandler>().BallBaseTrail.SetActive(false);
                            
                            VfxShootSpecialSpikeRed.Play();
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeRed.SetActive(true);
                        }
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
                else if (IsSpecialSpike && _playerNumberTouchBallHandler.NumberTouchBall < 2 && _isGrounded)
                {
                    IsSpecialSpike = false;
                    CanSpecialSpike = true;
                }

                if (IsSpecialSpike && _playerNumberTouchBallHandler.NumberTouchBall == 3)
                {
                    IsSpecialSpike = false;
                    CanSpecialSpike = true;
                }
            }
        }

        public void GetJoystickReadValue(Vector2 move)
        {
            if (_canMove)
            {
                _move = move;
            }
        }
        
        public void GetWestButtonReadValue(float buttonValue)
        {
            _isWestButtonPressed = buttonValue;
            
            if (Mathf.Approximately(buttonValue, 1) && _move == Vector2.zero && _isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 1)
            {
                _canPerfectReception = true;
                Invoke(nameof(ReverseCanPerfectReception), _timeCanPerfectReception);
            }

            if (Mathf.Approximately(buttonValue, 1) && !_isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 2)
            {
                CanAbsorb = true;
            }

            if (buttonValue == 0 && CanAbsorb)
            {
                CanAbsorb = false;
            }
            
            if (buttonValue == 0 && _hasTheBall && !IsSpecialSpike)
            {
                _ball.GetComponent<BallHandler>().IsShoot(_move);
                Invoke(nameof(ReverseHaveTheBall), 0.1f);
                
                FlipSpriteAbsorbDrawn(_move);
                
                // Shake Camera
                _cameraHandler.HitShake();
                
                // Animation
                _animator.SetTrigger("Drawn");
                // VFX
                if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
                {
                    if (PlayerType.PlayerName == "Bleu")
                    {
                        _vfxSpitOutBlue.Play();
                    }
                    if (PlayerType.PlayerName == "Vert")
                    {
                        _vfxSpitOutGreen.Play();
                    }
                    if (PlayerType.PlayerName == "Jaune")
                    {
                        _vfxSpitOutYellow.Play();
                    }
                    if (PlayerType.PlayerName == "Rouge")
                    {
                        _vfxSpitOutRed.Play();
                    }
                }
                
                // SFX
                _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                _audioSource.Play();
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (CanSpecialSpike && _playerNumberTouchBallHandler.IsPlayerOne == _ball.GetComponent<BallHandler>().IsPlayerOneSide)
                {
                    _ball.GetComponent<BallHandler>().SpecialSpikeActivation();

                    IsSpecialSpike = true;
                    CanAbsorb = true;
                    CanSpecialSpike = false;

                    if (_impactBackground)
                    {
                        if (PlayerType.PlayerName == "Bleu")
                        {
                            _impactBackground.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.71f, 0.87f);
                            ImpactBackgroundObject = Instantiate(_impactBackground, Vector2.zero, Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Vert")
                        {
                            _impactBackground.GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.77f, 0.28f);
                            ImpactBackgroundObject = Instantiate(_impactBackground, Vector2.zero, Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Jaune")
                        {
                            _impactBackground.GetComponent<SpriteRenderer>().color = new Color(0.96f, 0.86f, 0.44f);
                            ImpactBackgroundObject = Instantiate(_impactBackground, Vector2.zero, Quaternion.identity);
                        }
                        if (PlayerType.PlayerName == "Rouge")
                        {
                            _impactBackground.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.3f, 0.25f);
                            ImpactBackgroundObject = Instantiate(_impactBackground, Vector2.zero, Quaternion.identity);
                        }
                    }
                    
                    // Animation
                    _animator.SetTrigger("ActiveSpecialSpike");
                    // VFX
                    _vfxActiveSpecialSpike.Play();
                    if (_playerNumberTouchBallHandler.IsPlayerOne)
                    {
                        _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerOne();
                    }
                    else
                    {
                        _arenaVFXHandler.GetComponent<ArenaVFXHandler>().PlayWindPlayerTwo();
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }

                if (IsSpecialSpike && _hasTheBall)
                {
                    ActiveSpecialSpike();
                    FlipSpriteAbsorbDrawn(_move);
                    PerfectReceptionCount = 0;
                    
                    // Shake Camera
                    _cameraHandler.HitShake();
                    
                    // Animation
                    _animator.SetTrigger("Drawn");
                    // VFX
                    if (_vfxShootSpecialSpikeBlue && _vfxShootSpecialSpikeGreen && _vfxShootSpecialSpikeYellow)
                    {
                        if (PlayerType.PlayerName == "Bleu")
                        {
                            _ball.GetComponent<BallHandler>().BallBaseTrail.SetActive(false);
                            
                            _vfxShootSpecialSpikeBlue.Play();
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeBlue.SetActive(true);
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeBlueImpact.Play();
                        }
                        if (PlayerType.PlayerName == "Vert")
                        {
                            _ball.GetComponent<BallHandler>().BallBaseTrail.SetActive(false);
                            
                            _vfxShootSpecialSpikeGreen.Play();
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeGreen.SetActive(true);
                        }
                        if (PlayerType.PlayerName == "Jaune")
                        {
                            _ball.GetComponent<BallHandler>().BallBaseTrail.SetActive(false);
                            
                            _vfxShootSpecialSpikeYellow.Play();
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeYellow.SetActive(true);
                            _ball.GetComponent<BallHandler>().VFXSpecialSpikeYellowImpact.Play();
                        }
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                    return;
                }

                if (PlayerType.PlayerName == "Vert" && CountShootSpecialSpike == 1)
                {
                    ActiveSpecialSpike();
                    FlipSpriteAbsorbDrawn(_move);
                    
                    Debug.Log("Special Spike Green");
                    // VFX
                    if (_ball.GetComponent<BallHandler>().VFXSpecialSpikeGreen.activeSelf)
                    {
                        Debug.Log("Special Spike Green Play");
                        _ball.GetComponent<BallHandler>().VFXSpecialSpikeGreenImpact.Play();
                    }
                    return;
                }
            }
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            if (_isGrounded)
            {
                _canDoubleJump = false;
            }
            
            if (Mathf.Approximately(buttonValue, 1) && !_hasTheBall)
            {
                if (_canDoubleJump)
                { 
                    _rb2d.velocity = Vector2.zero;
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
                    
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = false;
                    
                    // Animation
                    _animator.SetTrigger("Jump");
                    // VFX
                    if (_vfxJumping)
                    {
                        _vfxJumping.Play();
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
                
                if (_isGrounded)
                { 
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    
                    // Animation
                    _animator.SetTrigger("Jump");
                    // VFX
                    if (_vfxJumping)
                    {
                        _vfxJumping.Play();
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
                
                if (_isWalled && !_isGrounded)
                {
                    Vector2 walljumping;
                    if (_move.x < -0.1f)
                    {
                        walljumping = new Vector2(1,1) * buttonValue * _wallJumpForce;
                    }
                    else
                    {
                        walljumping = new Vector2(-1,1) * buttonValue * _wallJumpForce;
                    }
                    
                    _rb2d.AddForce(walljumping, ForceMode2D.Impulse);
                    
                    // Animation
                    _animator.SetTrigger("WallJump");
                    // VFX
                    if (_vfxJumping)
                    {
                        _vfxJumping.Play();
                    }
                    
                    // SFX
                    _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                    _audioSource.Play();
                }
            }
        }

        public void GetStartButtonReadValue(float buttonValue)
        {
            if (Mathf.Approximately(buttonValue, 1))
            {
                GameManager.IsGamePaused = !GameManager.IsGamePaused;
            }
        }

        private void ActiveSpecialSpike()
        {
            if (_ball)
            {
                PlayerType.SpecialSpike(gameObject, _ball, _move);
            }
            
            // Animation
            _animator.SetTrigger("ShootSpecialSpike");
        }

        public void ResetStatesAfterSpecialSpike()
        {
            _hasTheBall = false;
            IsSpecialSpike = false;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Raycasts()
        {
            // Raycast _isGrounded
            RaycastHit2D hit2DGround = Physics2D.Raycast(transform.position, Vector3.down, _rayGroundedLength, _groundLayer);
            if (_isGrounded == false && hit2DGround)
            {
                if (_vfxSpitOutBlue && _vfxSpitOutGreen && _vfxSpitOutYellow && _vfxSpitOutRed)
                {
                    if (PlayerType.PlayerName == "Bleu")
                    {
                        _vfxLandingBlue.Play();
                    }
                    if (PlayerType.PlayerName == "Vert")
                    {
                        _vfxLandingGreen.Play();
                    }
                    if (PlayerType.PlayerName == "Jaune")
                    {
                        _vfxLandingYellow.Play();
                    }
                    if (PlayerType.PlayerName == "Rouge")
                    {
                        _vfxLandingRed.Play();
                    }
                }
                
                // SFX
                _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                _audioSource.Play();
            }
            _isGrounded = hit2DGround.collider;
            Debug.DrawRay(transform.position, Vector3.down * _rayGroundedLength, Color.red);

            _isWalled = false;
            if (-1 <= _move.x && _move.x <= -0.8)
            {
                // Raycast _isWalled
                RaycastHit2D hit2DWallLeft = Physics2D.Raycast(transform.position, Vector3.left, _rayWalledLength, _wallLayer);
                if (hit2DWallLeft)
                {
                    _isWalled = true;
                    _canDoubleJump = false;
                }
                Debug.DrawRay(transform.position, Vector3.left * _rayWalledLength, Color.red);
            }
            if (0.8 <= _move.x && _move.x <= 1)
            {
                // Raycast _isWalled
                RaycastHit2D hit2DWallRight = Physics2D.Raycast(transform.position, Vector3.right, _rayWalledLength, _wallLayer);
                if (hit2DWallRight)
                {
                    _isWalled = true;
                    _canDoubleJump = false;
                }
                Debug.DrawRay(transform.position, Vector3.right * _rayWalledLength, Color.red);
            }
        }

        private void Dash()
        {
            // Dash
            // Décompte du cooldown
            if (_dashCooldownRemaining > 0)
            {
                _dashCooldownRemaining -= Time.deltaTime;
            }
        
            // Déclencehment du dash
            if (Mathf.Approximately(_isWestButtonPressed, 1) && _dashCooldownRemaining <= 0 && _hasTheBall == false && _isGrounded && _move != Vector2.zero)
            {
                _isDashing = true;
                _dashTimeRemaining = _dashDuration;
                _dashCooldownRemaining = _dashCooldown;
                
                // SFX
                _audioSource.clip = AudioStock.Instance.BlopClips[Random.Range(0, AudioStock.Instance.BlopClips.Count)];
                _audioSource.Play();
            }
            
            if (_isDashing)
            {
                _canMove = false;

                if (transform.rotation.y <= 0)
                {
                    transform.Translate(_move.x * (_dashSpeed * Time.deltaTime), 0, 0);
                    _dashTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    transform.Translate(-_move.x * (_dashSpeed * Time.deltaTime), 0, 0);
                    _dashTimeRemaining -= Time.deltaTime;
                }
                
                if (_dashTimeRemaining <= 0)
                {
                    _isDashing = false;
                    _canMove = true;
                }
            }
        }

        
        // Utils
        private void ReverseHaveTheBall()
        {
            _hasTheBall = !_hasTheBall;
        }

        private void ReverseCanPerfectReception()
        {
            _canPerfectReception = !_canPerfectReception;
        }

        private void ReverseIsAttacking()
        {
            _isAttacking = !_isAttacking;
        }

        private void ReverseHasPunch()
        {
            _hasPunch = !_hasPunch;
        }
        
        private void ReverseCanMove()
        {
            _canMove = !_canMove;
        }

        private void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }

        private void ReverseAppears()
        {
            _appears = !_appears;
        }

        private void FlipSprite(float movement)
        {
            if (movement > 0.1f)
            {
                _sr.flipX = false;
            }
            else if (movement < -0.1f)
            {
                _sr.flipX = true;
            }
        }

        private void FlipSpriteAbsorbDrawn(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (_sr.flipX == false)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else
            {
                float normalizedAngle;
                if (angle < 0)
                {
                    normalizedAngle = (angle + 180f) % 180f;
                }
                else
                {
                    normalizedAngle = (angle - 180f) % 180f;
                }
                transform.rotation = Quaternion.Euler(0f, 0f, normalizedAngle);
            }
            Invoke(nameof(ResetRotation), _timeResetRotation);
        }

        public void PlayerDie()
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            
            // Animation
            _animator.SetTrigger("Die");
            
            Destroy(gameObject, 0.3f);
        }
    }
}