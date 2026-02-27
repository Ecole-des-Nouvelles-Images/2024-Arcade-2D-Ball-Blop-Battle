using System;
using Hugo.Prototype.Scripts.Player;
using Int.Scripts.Utils;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class NewBlopController : MonoBehaviour
    {
        public int PlayerId;
        
        [Header("Blop")]
        [SerializeField] private Blop _blop;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Animator _animator;
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
        private float _perfectReceptionCount;
        private float _perfectReceptionTimeRemaining;
        private float _perfectReceptionCooldownRemaining;
        
        // BALL
        private BallController _ballController;

        public void SetUp(Blop blop, int playerId)
        {
            _blop = blop;
            PlayerId = playerId;
            _animator.runtimeAnimatorController = _blop.PlayerAnimatorController;
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
            
            // FOUL
            if (_hasTheBall && _isGrounded)
            {
                NewMatchManager.Instance.Foul(PlayerId);
                _hasTheBall = false;
                _isAbsorbing = false;
            }
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
                }
                else if (_isDashing)
                {
                    _ballController.DashReception();
                }
                else if (_isAbsorbing)
                {
                    _hasTheBall = true;
                    _ballController.Absorb(transform);
                }
                else if (_isSpecialSpike)
                {
                    
                }
                
                EventBus.OnPlayerTouchedBall?.Invoke(PlayerId);
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
                }
                else if (!_isPerfectReception && _isGrounded && _perfectReceptionCooldownRemaining <= 0
                         && Mathf.Abs(_move.x) < 0.1f)
                {
                    _isPerfectReception = true;
                    _perfectReceptionTimeRemaining = _blop.PerfectReceptionDuration;
                    _perfectReceptionCooldownRemaining = _blop.PerfectReceptionCooldown;
                }

                if (!_isGrounded && !_isWalledLeft && !_isWalledRight)
                {
                    _isAbsorbing = true;
                }
            }
            else if (Mathf.Approximately(buttonValue, 0))
            {
                _isAbsorbing = false;
                
                if (_hasTheBall)
                {
                    if (_ballController)
                    {
                        _ballController.Drawn(_move);
                        _ballController = null;
                    }
                    
                    _hasTheBall = false;
                }
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            // Debug.Log(buttonValue);
            if (Mathf.Approximately(buttonValue, 1))
            {
                if (_canSpecialSpike && PlayerId == NewMatchManager.Instance.BallSide)
                {
                    _isSpecialSpike = true;
                    _perfectReceptionCount = 0;
                    EventBus.OnSpecialSpikeActivated?.Invoke();
                }
            }
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            // Debug.Log(buttonValue);
            if (_hasTheBall) return;

            if (_isGrounded || _isWalledLeft || _isWalledRight)
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
                }
                else if (_isWalledLeft)
                {
                    Vector2 wallJumping = new Vector2(1,1) * _blop.WallJumpForce;
                    _rb2d.velocity = new Vector3(wallJumping.x, wallJumping.y);
                }
                else if (_isWalledRight)
                {
                    Vector2 wallJumping = new Vector2(-1,1) * _blop.WallJumpForce;
                    _rb2d.velocity = new Vector3(wallJumping.x, wallJumping.y);
                }
                else if (_canDoubleJump)
                {
                    float jumping = 1f * _blop.JumpForce;
                    _rb2d.velocity = new Vector3(_rb2d.velocity.x, jumping);
                    _canDoubleJump = false;
                }
            }
        }

        public void GetStartButtonReadValue(float buttonValue)
        {
            Debug.Log(buttonValue);
        }

        private void Raycasts()
        {
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
            
            _isWalledLeft = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector3.left, 
                _blop.RayWalledLength, _blop.WallLayer);
            
            _isWalledRight = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector3.right, 
                _blop.RayWalledLength, _blop.WallLayer);
            
            // DEBUG
            if (!_hasTheBall)
            {
                Debug.DrawRay(transform.position, Vector3.down * _blop.RayGroundedLength, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, Vector3.down * _blop.RayGroundedLengthHaveTheBall, Color.red);

            }
            Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.left * _blop.RayWalledLength, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.right * _blop.RayWalledLength, Color.red);
        }

        #region === EVENTS ===

        private void OnEnable()
        {
            _laserTrigger.SetActive(true);
            
            EventBus.OnFoul += Foul;
        }

        private void Foul()
        {
            _hasTheBall = false;
        }
        
        private void OnDisable()
        {
            EventBus.OnFoul -= Foul;
        }

        #endregion
    }
}
