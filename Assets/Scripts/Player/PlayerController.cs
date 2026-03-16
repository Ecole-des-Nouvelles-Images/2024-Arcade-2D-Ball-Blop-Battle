using System;
using Balls;
using Managers;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public int PlayerId;
        
        // PROPERTIES
        public PlayerEvents PlayerEvents { get; private set; } =  new ();
        public BlopType BlopType { get; private set; }
        
        [Header("Blop")]
        [SerializeField] private Blop _blop;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerCountTouchBall _playerCountTouchBall;
        [SerializeField] private GameObject _laserTrigger;
        
        [Header("States")]
        [SerializeField] private bool _hasTheBall;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isWalledLeft;
        [SerializeField] private bool _isWalledRight;
        [SerializeField] private bool _isDashing;
        [SerializeField] private bool _isAbsorbing;
        [SerializeField] private bool _isPerfectReception;
        [SerializeField] private bool _isSpecialSpike;
        [SerializeField] private bool _isPunchingBall;
        
        [Header("Permissions")]
        [SerializeField] private bool _canMove = true;
        [SerializeField] private bool _canDoubleJump;
        [SerializeField] private bool _canSpecialSpike => _perfectReceptionCount >= 3;

        // MOVEMENT
        private Vector2 _move;
        
        // DASH
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;
        
        // PERFECT RECEPTION
        private int _perfectReceptionCount;
        private float _perfectReceptionTimeRemaining;
        private float _perfectReceptionCooldownRemaining;
        
        // BALL
        private BallController _ballController;

        public void SetUp(Blop blop, int playerId)
        {
            _blop = blop;
            PlayerId = playerId;
            _animator.runtimeAnimatorController = _blop.PlayerAnimatorController;
            Instantiate(_blop.PSTrailRenderer, transform);
            
            BlopType = blop.BlopType;
        }
        
        public void DebugSetUp(int playerId)
        {
            PlayerId = playerId;
            _animator.runtimeAnimatorController = _blop.PlayerAnimatorController;
            Instantiate(_blop.PSTrailRenderer, transform);
            
            BlopType = _blop.BlopType;
        }

        public void Die()
        {
            // RESET
            gameObject.SetActive(false);
            _move = Vector2.zero;
            _hasTheBall = false;
            _isAbsorbing = false;
            
            EventBus.OnPlayerDie?.Invoke(PlayerId);
        }

        public void ResetSpecialSpikeState()
        {
            _hasTheBall = false;
            _isSpecialSpike = false;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        private void Update()
        {
            Raycasts();
            
            // PERFECT RECEPTION
            if (_perfectReceptionCooldownRemaining >= 0)
            {
                _perfectReceptionCooldownRemaining -= Time.deltaTime;
            }
            
            if (_isPerfectReception)
            {
                _perfectReceptionTimeRemaining -= Time.deltaTime;
                
                if (_perfectReceptionTimeRemaining <= 0)
                {
                    _isPerfectReception = false;
                }
            }
            
            if (_isGrounded && _isAbsorbing)
            {
                _isAbsorbing = false;
            }
            
            if (!_isGrounded)
            {
                _isPerfectReception = false;
            }
            
            // FOUL
            if (_hasTheBall && _isGrounded)
            {
                MatchManager.Instance.Foul(PlayerId);
                _hasTheBall = false;
                _isAbsorbing = false;
            }
            
            // ANIMATOR
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("IsWalled", _isWalledLeft || _isWalledRight);
            _animator.SetBool("IsDashing", _isDashing);
            _animator.SetBool("IsPerfectReception", _isPerfectReception);
            _animator.SetBool("IsPunchingBall", _isPunchingBall);
            _animator.SetBool("IsAbsorbing", _isAbsorbing);
            _animator.SetBool("HasTheBall", _hasTheBall);
        }

        private void FixedUpdate()
        {
            // COOLDOWN
            if (_dashCooldownRemaining >= 0)
            {
                _dashCooldownRemaining -= Time.fixedDeltaTime;
            }
            
            // DASH
            if (_isDashing)
            {
                Vector2 movement;
                if (_move.x > 0f)
                {
                    movement = new Vector2(_blop.DashSpeed * Time.fixedDeltaTime, _rb2d.velocity.y);
                }
                else
                {
                    movement = new Vector2(-_blop.DashSpeed * Time.fixedDeltaTime, _rb2d.velocity.y);
                }
                _rb2d.velocity = movement;
                _dashTimeRemaining -= Time.fixedDeltaTime;
                
                if (_dashTimeRemaining <= 0)
                {
                    _isDashing = false;
                    _canMove = true;
                }
                return;
            }
            
            // MOVEMENT
            if (_canMove && !_hasTheBall)
            {
                if (_isGrounded)
                {
                    Vector2 movement = new Vector2(_move.x * (_blop.Speed * Time.fixedDeltaTime), _rb2d.velocity.y);
                    _rb2d.velocity = movement;
                }
                else
                {
                    float airSpeed = _move.x * _blop.Speed * _blop.AirControlFactor;
                    float newMovement = Math.Clamp(_rb2d.velocity.x + airSpeed * Time.fixedDeltaTime, -_blop.MaxAirSpeed, _blop.MaxAirSpeed);
                    _rb2d.velocity = new Vector2(newMovement, _rb2d.velocity.y);
                }
            }
            
            FlipSprite(_move.x);
            
            // ANIMATOR
            _animator.SetFloat("VelocityX", Mathf.Abs(_rb2d.velocity.x));
            _animator.SetFloat("VelocityY", _rb2d.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ballController = other.gameObject.GetComponent<BallController>();
                
                if (_isPerfectReception)
                {
                    _ballController.PerfectReception();
                    _perfectReceptionCount = Mathf.Clamp(_perfectReceptionCount + 1, 0, 3);
                    
                    EventBus.OnPlayerPerfectReception?.Invoke(PlayerId, _perfectReceptionCount);
                    
                    PlayerEvents.PerfectReception(_blop);
                }
                else if (_isDashing)
                {
                    _ballController.DashReception();
                }
                else if (_isAbsorbing)
                {
                    _hasTheBall = true;
                    _ballController.Absorb(transform);
                    
                    PlayerEvents.Absorb(_blop, _ballController.transform);
                }
                else if (_isSpecialSpike)
                {
                    _hasTheBall = true;
                    _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    _ballController.Absorb(transform);
                    
                    PlayerEvents.AbsorbSpecialSpike(_blop, _ballController.transform);
                }
                else
                {
                    _isPunchingBall = true;
                    Invoke(nameof(ReverseIsPunchingBall), 0.2f);
                }
                
                EventBus.OnPlayerTouchedBall?.Invoke(PlayerId);
                PlayerEvents.Punch(_blop);
            }
        }

        public void GetJoystickReadValue(Vector2 move)
        {
            // Debug.Log(move);
            if (_canMove)
            {
                _move = move;
            }
        }
        
        public void GetWestButtonReadValue(float buttonValue)
        {
            // Debug.Log(buttonValue);
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (!_isDashing && _isGrounded && _dashCooldownRemaining <= 0
                    && Mathf.Abs(_move.x) >= 0.1f)
                {
                    _isDashing = true;
                    _dashTimeRemaining = _blop.DashDuration;
                    _dashCooldownRemaining = _blop.DashCooldown;
                
                    _canMove = false;
                    
                    PlayerEvents.Dash(_blop);
                }
                else if (!_isPerfectReception && _isGrounded && _perfectReceptionCooldownRemaining <= 0
                         && Mathf.Abs(_move.x) < 0.1f && _playerCountTouchBall.CurrentTouchCount == 0 && !_isSpecialSpike)
                {
                    _isPerfectReception = true;
                    _perfectReceptionTimeRemaining = _blop.PerfectReceptionDuration;
                    _perfectReceptionCooldownRemaining = _blop.PerfectReceptionCooldown;
                }

                if (!_isGrounded && !_isWalledLeft && !_isWalledRight && !_hasTheBall)
                {
                    _isAbsorbing = true;
                    
                    PlayerEvents.CanAbsorb(_blop);
                }
            }
            else if (Mathf.Approximately(buttonValue, 0) && !_isSpecialSpike)
            {
                _isAbsorbing = false;
                
                if (_hasTheBall)
                {
                    if (_ballController)
                    {
                        _ballController.Drawn(_move);
                        
                        PlayerEvents.Drawn(_blop, _ballController.transform);
                    }
                    
                    _hasTheBall = false;
                    _ballController = null;
                    
                    // ANIMATOR
                    _animator.SetTrigger("DrawnBall");
                }
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            // Debug.Log(buttonValue);
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (_canSpecialSpike && PlayerId == MatchManager.Instance.BallSide)
                {
                    _isSpecialSpike = true;
                    _perfectReceptionCount = 0;
                    
                    EventBus.OnSpecialSpikeActivated?.Invoke();
                    EventBus.OnPlayerPerfectReception?.Invoke(PlayerId, _perfectReceptionCount);
                    
                    PlayerEvents.ActiveSpecialSpike(_blop);
                }

                if (_isSpecialSpike && _hasTheBall)
                {
                    if (_ballController)
                    {
                        Debug.Log("SPECIAL SPIKE");
                        _blop.SpecialSpike(gameObject, _ballController.gameObject, _move);
                        _ballController = null;
                        
                        PlayerEvents.ShootSpecialSpike(_blop);
                    }
                }
            }
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            // Debug.Log(buttonValue);
            if (_hasTheBall) return;

            if (_isGrounded)
            {
                _canDoubleJump = false;
            }
            
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (_isGrounded)
                {
                    float jumping = 1f * _blop.JumpForce;
                    _rb2d.velocity = new Vector3(_rb2d.velocity.x, jumping);
                    _canDoubleJump = true;
                    
                    PlayerEvents.Jump(_blop);
                    
                    // ANIMATOR
                    _animator.SetTrigger("Jumping");
                }
                else if (_isWalledLeft)
                {
                    Vector2 wallJumping = new Vector2(1,1) * _blop.WallJumpForce;
                    _rb2d.velocity = new Vector3(wallJumping.x, wallJumping.y);
                    _canDoubleJump = true;
                    
                    PlayerEvents.WallJump(_blop);
                    
                    // ANIMATOR
                    _animator.SetTrigger("JumpingWall");
                }
                else if (_isWalledRight)
                {
                    Vector2 wallJumping = new Vector2(-1,1) * _blop.WallJumpForce;
                    _rb2d.velocity = new Vector3(wallJumping.x, wallJumping.y);
                    _canDoubleJump = true;
                    
                    PlayerEvents.WallJump(_blop);
                    
                    // ANIMATOR
                    _animator.SetTrigger("JumpingWall");
                }
                else if (_canDoubleJump)
                {
                    float jumping = 1f * _blop.JumpForce;
                    _rb2d.velocity = new Vector3(_rb2d.velocity.x, jumping);
                    _canDoubleJump = false;
                    
                    PlayerEvents.DoubleJump(_blop);
                    
                    // ANIMATOR
                    _animator.SetTrigger("Jumping");
                }
            }
        }

        public void GetStartButtonReadValue(float buttonValue)
        {
            Debug.Log(buttonValue);
        }

        private void Raycasts()
        {
            bool lastIsGrounded = _isGrounded;
            bool lastIsWalledLeft = _isWalledLeft;
            bool lastIsWalledRight = _isWalledRight;
            
            if (!_hasTheBall)
            {
                _isGrounded = Physics2D.Raycast(transform.position, Vector3.down, 
                    _blop.RayGroundedLength, _blop.GroundLayer);
            }
            else
            {
                _isGrounded = Physics2D.Raycast(transform.position, Vector3.down, 
                    _blop.RayGroundedLengthHaveTheBall, _blop.GroundLayer);
            }

            if (_move.x < -0.1f)
                _isWalledLeft = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0),
                    Vector3.left, _blop.RayWalledLength, _blop.WallLayer);
            else _isWalledLeft = false;
            
            if (_move.x > 0.1f) _isWalledRight = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), 
                Vector3.right, _blop.RayWalledLength, _blop.WallLayer);
            else _isWalledRight = false;
            
            // EVENTS
            if (_isGrounded && lastIsGrounded != _isGrounded)
            {
                PlayerEvents.Land(_blop);
            }

            if (_isWalledLeft && lastIsWalledLeft != _isWalledLeft 
                || _isWalledRight && lastIsWalledRight != _isWalledRight)
            {
                PlayerEvents.IsWalled(_blop);
            }
            
            // DEBUG
            if (!_hasTheBall)
            {
                Debug.DrawRay(transform.position, Vector3.down * _blop.RayGroundedLength, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.down * _blop.RayGroundedLengthHaveTheBall, Color.red);

            }
            if (_move.x < -0.1f) Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.left * _blop.RayWalledLength, Color.red);
            if (_move.x > 0.1f) Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.right * _blop.RayWalledLength, Color.red);
        }

        private void ReverseIsPunchingBall()
        {
            _isPunchingBall = false;
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            ResetSpecialSpikeState();
            _laserTrigger.SetActive(true);
            
            EventBus.OnPlayerScored += PlayerScored;
            EventBus.OnFoul += Foul;
            EventBus.OnMatchOver += MatchOver;
            
            PlayerEvents.Appears(_blop);
        }

        private void PlayerScored(int playerId)
        {
            ResetSpecialSpikeState();
        }

        private void Foul()
        {
            ResetSpecialSpikeState();
        }
        
        private void MatchOver(int playerId)
        {
            // ANIMATOR
            if (PlayerId == playerId)
            {
                _animator.SetTrigger("WinMatch");
            }
            else
            {
                _animator.SetTrigger("LoseMatch");
            }
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= PlayerScored;
            EventBus.OnFoul -= Foul;
            EventBus.OnMatchOver -= MatchOver;

            // Instantiate(_blop.PSDeath, transform.position, transform.rotation);
        }

        #endregion
        
        #region ===== ANIMATOR =====
        
        private void FlipSprite(float movement)
        {
            if (_isWalledLeft || _isWalledRight) return;
            
            if (movement > 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
            }
            else if (movement < -0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 180, transform.rotation.eulerAngles.z);
            }
        }
        
        #endregion
    }
}
