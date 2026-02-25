using System;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class NewBlopController : MonoBehaviour
    {
        [Header("Blop")]
        [SerializeField] private Blop _blop;
        
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Animator _animator;
        
        [Header("States")]
        [SerializeField] private bool _hasTheBall;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private bool _isWalledLeft;
        [SerializeField] private bool _isWalledRight;
        [SerializeField] private bool _isDashing;
        [SerializeField] private bool _isAbsorbing;
        
        [Header("Permissions")]
        [SerializeField] private bool _canMove = true;
        [SerializeField] private bool _canDoubleJump;
        [SerializeField] private bool _canPerfectReception;

        // MOVEMENT
        private Vector2 _move;
        
        // DASH
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;
        
        // PERFECT RECEPTION
        private float _perfectReceptionTimeRemaining;
        private float _perfectReceptionCooldownRemaining;
        
        // BALL
        private BallController _ballController;

        public void SetUp(Blop blop)
        {
            _blop = blop;
            _animator.runtimeAnimatorController = _blop.PlayerAnimatorController;
        }
        
        private void Update()
        {
            Raycasts();
            
            // PERFECT RECEPTION
            if (_perfectReceptionCooldownRemaining >= 0)
            {
                _perfectReceptionCooldownRemaining -= Time.deltaTime;
            }
            
            if (_canPerfectReception)
            {
                _perfectReceptionTimeRemaining -= Time.deltaTime;
                
                if (_perfectReceptionTimeRemaining <= 0)
                {
                    _canPerfectReception = false;
                }
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
                
                if (_canPerfectReception)
                {
                    _ballController.PerfectReception();
                }
                else if (_isDashing)
                {
                    _ballController.DashReception();
                }
                else if (_isAbsorbing)
                {
                    _ballController.Absorb(transform);
                    _rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
                }
            }
        }

        public void GetJoystickReadValue(Vector2 move)
        {
            Debug.Log(move);
            if (_canMove)
            {
                _move = move;
            }
        }
        
        public void GetWestButtonReadValue(float buttonValue)
        {
            Debug.Log(buttonValue);
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
                else if (!_canPerfectReception && _isGrounded && _perfectReceptionCooldownRemaining <= 0
                         && Mathf.Abs(_move.x) < 0.1f)
                {
                    _canPerfectReception = true;
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
                if (_isAbsorbing)
                {
                    if (_ballController)
                    {
                        _ballController.Drawn(_move);
                        _ballController = null;
                    }

                    _isAbsorbing = false;
                    
                    _rb2d.constraints = RigidbodyConstraints2D.None;
                    _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            Debug.Log(buttonValue);
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            Debug.Log(buttonValue);
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
            _isGrounded = Physics2D.Raycast(transform.position, Vector3.down, 
                _blop.RayGroundedLength, _blop.GroundLayer);
            
            _isWalledLeft = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector3.left, 
                _blop.RayWalledLength, _blop.WallLayer);
            
            _isWalledRight = Physics2D.Raycast(transform.position + new Vector3(0, .5f, 0), Vector3.right, 
                _blop.RayWalledLength, _blop.WallLayer);
            
            // DEBUG
            Debug.DrawRay(transform.position, Vector3.down * _blop.RayGroundedLength, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.left * _blop.RayWalledLength, Color.red);
            Debug.DrawRay(transform.position + new Vector3(0, .5f, 0), Vector3.right * _blop.RayWalledLength, Color.red);
        }
    }
}
