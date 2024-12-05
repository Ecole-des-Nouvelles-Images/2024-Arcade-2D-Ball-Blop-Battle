using System;
using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Game;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Special Spike
        public int CountShootSpecialSpike;
        
        // Components
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        private PlayerNumberTouchBallHandler _playerNumberTouchBallHandler;
        private Animator _animator;
        
        // GameObject
        private GameObject _ball;
        
        // States
        private bool _hasTheBall;
        private bool _isDashing;
        private bool _canPerfectReception;
        private bool _canMove = true;
        private bool _isGrounded;
        private bool _isWalled;
        private bool _canDoubleJump;
        private bool _canAbsorb;

        // Inputs values
        private Vector2 _move;
        private float _isWestButtonPressed;

        // Special spike
        public int PerfectReceptionCount;
        private bool _canSpecialSpike;
        private bool _isSpecialSpike;
        private bool _shootSpecialSpike;
        
        // Player Type
        [Header("Player Type")]
        [SerializeField]
        private PlayerType _playerType;
        
        // Player Settings
        [Header("Player Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _wallJumpForce;
        [SerializeField] private float _jumpingSpeed;
        [SerializeField] private float _airControlFactor;
        [SerializeField] private float _maxAirSpeed;
        [SerializeField] private float _timePerfectReception;
        
        
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

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _playerNumberTouchBallHandler = GetComponent<PlayerNumberTouchBallHandler>();
            _animator = GetComponent<Animator>();
            
            _playerType = _playerNumberTouchBallHandler.IsPlayerOne ? GameManager.FirstPlayerScriptableObject : GameManager.SecondPlayerScriptableObject;
        }

        private void Start()
        {
            _sr.sprite = _playerType.Sprite;
            _animator.runtimeAnimatorController = _playerType.PlayerAnimatorController;
        }

        private void Update()
        {
            Raycasts();
            Dash();
            
            _ball = GameObject.FindGameObjectWithTag("Ball");
            
            // Faute when _hasTheBall && _isGrounded
            if (_isGrounded && _hasTheBall)
            {
                _playerNumberTouchBallHandler.Fouls();
                _hasTheBall = false;
            }
            
            // Reset States End of Timer
            if (MatchManager.IsSetOver)
            {
                _hasTheBall = false;
                _isSpecialSpike = false;
            }
            
            // Animation
            _animator.SetBool("HasTheBall", _hasTheBall);
            _animator.SetBool("IsWalled", _isWalled);
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("CanAbsorb", _canAbsorb);
            _animator.SetFloat("GoUp", _rb2d.velocity.y);
            _animator.SetFloat("MaxJumpHeight", _rb2d.velocity.y);
            _animator.SetFloat("Falling", _rb2d.velocity.y);
        }

        private void FixedUpdate()
        {
            if (!_hasTheBall)
            {
                _sr.color = new Color(1, 1, 1, 1f);
                
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
                _sr.color = new Color(1, 1, 1, 0.5f);
            }
            
            // Animation
            _animator.SetFloat("Speed", Mathf.Abs(_rb2d.velocity.x));
            FlipSprite(_rb2d.velocity.x);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                if (_canPerfectReception)
                {
                    if (_move == Vector2.zero && _isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 1)
                    {
                        _ball.GetComponent<BallHandler>().PerfectReception();
                        PerfectReceptionCount++;
                        
                        // Animation
                        _animator.SetTrigger("PerfectReception");
                            
                        if (PerfectReceptionCount == 3)
                        {
                            _canSpecialSpike = true;
                        }
                    }
                }
                
                if (Mathf.Approximately(_isWestButtonPressed, 1) && !_isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 2)
                { 
                    _ball.GetComponent<BallHandler>().IsAbsorb(gameObject);
                    _hasTheBall = true;
                    _isDashing = false;
                    _canMove = true;
                    
                    // Animation
                    _animator.SetTrigger("Absorb");
                }
                else
                {
                    Vector2 direction = new Vector2(_ball.transform.position.x - transform.position.x, _ball.transform.position.y - transform.position.y);
                    _ball.GetComponent<BallHandler>().IsPunch(direction, _rb2d.velocity);
                    
                    // Animation
                    _animator.SetTrigger("Attack");
                }

                if (_isSpecialSpike && _playerNumberTouchBallHandler.NumberTouchBall < 2)
                {
                    _hasTheBall = true;
                    _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    _ball.GetComponent<BallHandler>().IsAbsorb(gameObject);
                    
                    // Animation
                    _animator.SetTrigger("Absorb");
                }

                if (_isSpecialSpike && _playerNumberTouchBallHandler.NumberTouchBall == 3)
                {
                    _isSpecialSpike = false;
                    _canSpecialSpike = true;
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
            
            if (Mathf.Approximately(buttonValue, 1))
            {
                _canPerfectReception = true;
                Invoke(nameof(ReverseCanPerfectReception), _timePerfectReception);
            }

            if (Mathf.Approximately(buttonValue, 1) && !_isGrounded && _playerNumberTouchBallHandler.NumberTouchBall < 2)
            {
                _canAbsorb = true;
            }

            if (buttonValue == 0 && _canAbsorb)
            {
                _canAbsorb = false;
            }
            
            if (buttonValue == 0 && _hasTheBall && !_isSpecialSpike)
            {
                _ball.GetComponent<BallHandler>().IsShoot(_move);
                Invoke(nameof(ReverseHaveTheBall), 0.1f);
                
                // Animation
                _animator.SetTrigger("Drawn");
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (_canSpecialSpike && _playerNumberTouchBallHandler.IsPlayerOne == _ball.GetComponent<BallHandler>().IsPlayerOneSide)
                {
                    _ball.GetComponent<BallHandler>().SpecialSpikeActivation();

                    _isSpecialSpike = true;
                    _canSpecialSpike = false;
                    PerfectReceptionCount = 0;
                    
                    // Animation
                    _animator.SetTrigger("ActiveSpecialSpike");
                }

                if (_isSpecialSpike && _hasTheBall)
                {
                    ActiveSpecialSpike();
                    return;
                }

                if (_playerType.name == "Vert" && CountShootSpecialSpike == 1)
                {
                    ActiveSpecialSpike();
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
                }
                
                if (_isGrounded)
                { 
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    
                    // Animation
                    _animator.SetTrigger("Jump");
                }
                
                if (_isWalled && !_isGrounded)
                {
                    Vector2 walljumping = new Vector2(1,1) * buttonValue * _wallJumpForce;
                    
                    _rb2d.AddForce(walljumping, ForceMode2D.Impulse);
                    
                    // Animation
                    _animator.SetTrigger("WallJump");
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
                _playerType.SpecialSpike(gameObject, _ball, _move);
            }
            
            // Animation
            _animator.SetTrigger("ShootSpecialSpike");
        }

        public void ResetStatesAfterSpecialSpike()
        {
            _hasTheBall = false;
            _isSpecialSpike = false;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void Raycasts()
        {
            // Raycast _isGrounded
            RaycastHit2D hit2DGround = Physics2D.Raycast(transform.position, Vector3.down, _rayGroundedLength, _groundLayer);
            _isGrounded = hit2DGround.collider;
            Debug.DrawRay(transform.position, Vector3.down * _rayGroundedLength, Color.red);

            _isWalled = false;
            if (-1 <= _move.x && _move.x <= -0.8 || 0.8 <= _move.x && _move.x <= 1)
            {
                // Raycast _isWalled
                RaycastHit2D hit2DWallRight = Physics2D.Raycast(transform.position, Vector3.right, _rayWalledLength, _wallLayer);
                RaycastHit2D hit2DWallLeft = Physics2D.Raycast(transform.position, Vector3.left, _rayWalledLength, _wallLayer);
                if (hit2DWallLeft || hit2DWallRight)
                {
                    _isWalled = true;
                    _canDoubleJump = false;
                }
                Debug.DrawRay(transform.position, Vector3.right * _rayWalledLength, Color.red);
                Debug.DrawRay(transform.position, Vector3.left * _rayWalledLength, Color.red);
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
                
                // Animation
                _animator.SetTrigger("Dash");
            }
            
            if (_isDashing)
            {
                _sr.color = Color.blue;
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
        
        private void ReverseCanMove()
        {
            _canMove = !_canMove;
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

        public void PlayerDie()
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            
            // Animation
            _animator.SetTrigger("Die");
            
            Destroy(gameObject, 0.3f);
        }
    }
}