using System;
using Hugo.Prototype.Scripts.Ball;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        private PlayerInput _playerInput;
        
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
        private bool _isOnTheNet;
        
        // Inputs values
        private Vector2 _move;
        private float _isWestButtonPressed;
        private float _isSouthButtonPressed;
        
        // Special spike
        private int _specialSpikeCount;
        private bool _canSpecialSpike;
        private bool _isSpecialSpike;
        
        // Player Type
        [Header("Player Type")]
        [SerializeField]
        private PlayerData _playerData;
        
        // Player Settings
        [Header("Player Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _jumpingSpeed;
        [SerializeField] private float _airControlFactor;
        [SerializeField] private float _maxAirSpeed;
        [SerializeField] private float _timePerfectReception;
        [SerializeField] private float _durationSpecialSpike;
        
        
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
        [SerializeField] private float _rayNetTouchedLength;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private LayerMask _netLayer;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _playerInput = GetComponent<PlayerInput>();
            // if (_playerInput.playerIndex == 0)
            // {
            //     _playerType = GameManager.FirstPlayerScriptableObject;
            // }
            // else
            // {
            //     _playerType = GameManager.SecondPlayerScriptableObject;
            // }
        }

        private void Start()
        {
            _sr.sprite = _playerData.Sprite;
        }

        private void Update()
        {
            // Raycast _isGrounded
            RaycastHit2D hit2DGround = Physics2D.Raycast(transform.position, Vector3.down, _rayGroundedLength, _groundLayer);
            _isGrounded = hit2DGround.collider;
            
            // Raycast _isOnTheNet
            RaycastHit2D hit2DNet = Physics2D.Raycast(transform.position, Vector3.down, _rayNetTouchedLength, _netLayer);
            _isOnTheNet = hit2DNet.collider;
            
            Debug.DrawRay(transform.position, Vector3.down * _rayGroundedLength, Color.red);

            _isWalled = false;
            if (-1 <= _move.x && _move.x <= -0.8 || 0.8 <= _move.x && _move.x <= 1)
            {
                // Raycast _isWalled
                RaycastHit2D hit2DWallRight = Physics2D.Raycast(transform.position, Vector3.right, _rayGroundedLength, _wallLayer);
                RaycastHit2D hit2DWallLeft = Physics2D.Raycast(transform.position, Vector3.left, _rayGroundedLength, _wallLayer);
                if (hit2DWallLeft || hit2DWallRight)
                {
                    _isWalled = true;
                }
                
                Debug.DrawRay(transform.position, Vector3.right * _rayGroundedLength, Color.red);
                Debug.DrawRay(transform.position, Vector3.left * _rayGroundedLength, Color.red);
            }
            
            // Debug.Log(_isGrounded);
            // Debug.Log(_isWalled);
            
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

            if (_isOnTheNet)
            {
                Debug.Log(" FAUTE : Net touched ! ");
                Destroy(gameObject);
            }
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

            if (_move.x > 0)
            {
                var rotation = transform.rotation;
                rotation.y = 0;
                transform.rotation = rotation;
            }
            else
            {
                var rotation = transform.rotation;
                rotation.y = 180;
                transform.rotation = rotation;
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                if (_canPerfectReception)
                {
                    if (_move == Vector2.zero && _isGrounded)
                    {
                        _ball.GetComponent<BallHandler>().PerfectReception();
                        _specialSpikeCount++;
                        Debug.Log(_specialSpikeCount + " perfect reception ! ");
                            
                        if (_specialSpikeCount == 3)
                        {
                            _specialSpikeCount = 0;
                            _canSpecialSpike = true;
                            Debug.Log(_canSpecialSpike);
                        }
                    }
                }
                
                if (Mathf.Approximately(_isWestButtonPressed, 1) && !_isGrounded)
                { 
                    _ball.GetComponent<BallHandler>().IsCatch(gameObject);
                    _hasTheBall = true;
                    _isDashing = false;
                    _canMove = true;
                }
                else
                {
                    Vector2 direction = new Vector2(_ball.transform.position.x - transform.position.x, _ball.transform.position.y - transform.position.y);
                    _ball.GetComponent<BallHandler>().IsPunch(direction, _rb2d.velocity);
                }

                if (_isSpecialSpike)
                {
                    _hasTheBall = true;
                    _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                    _ball.GetComponent<BallHandler>().IsCatch(gameObject);

                    Invoke(nameof(ActiveSpecialSpike), _durationSpecialSpike);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("AboveNet"))
            {
                Debug.Log(" FAUTE : Above Net ! ");
                Destroy(gameObject);
            }
        }

        public void GetJoystickReadValue(Vector2 move)
        {
            if (_canMove)
            {
                _move = move;
                //Debug.Log(_move);
            }
        }
        
        public void GetWestButtonReadValue(float buttonValue)
        {
            _isWestButtonPressed = buttonValue;
            //Debug.Log(_isWestButtonPressed);
            
            if (Mathf.Approximately(buttonValue, 1) && Mathf.Approximately(_isSouthButtonPressed, 1) && _canSpecialSpike)
            {
                _ball.GetComponent<BallHandler>().SpecialSpikeActivation();

                _isSpecialSpike = true;
                _canSpecialSpike = false;
                
                return;
            }
            
            if (Mathf.Approximately(buttonValue, 1))
            {
                _canPerfectReception = true;
                Invoke(nameof(ReverseCanPerfectReception), _timePerfectReception);
            }
            
            if (buttonValue == 0 && _hasTheBall)
            {
                _ball.GetComponent<BallHandler>().IsDrawn(_move);
                Invoke(nameof(ReverseHaveTheBall), 0.1f);
            }
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            _isSouthButtonPressed = buttonValue;
            //Debug.Log(_isSouthButtonPressed);

            if (_isGrounded || _isWalled)
            {
                _canDoubleJump = false;
            }
            
            if (Mathf.Approximately(buttonValue, 1) && Mathf.Approximately(_isWestButtonPressed, 1) && _canSpecialSpike)
            {
                _ball.GetComponent<BallHandler>().SpecialSpikeActivation();

                _isSpecialSpike = true;
                _canSpecialSpike = false;
                
                return;
            }
            
            if (Mathf.Approximately(buttonValue, 1) && !_hasTheBall)
            {
                if (_canDoubleJump)
                { 
                    _rb2d.velocity = Vector2.zero;
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
                    //Debug.Log(" Second Jump ! ");
                    
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = false;
                }
                
                if (_isGrounded && !_isWalled || _isGrounded && _isWalled)
                { 
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
                    //Debug.Log(" First Jump ! ");
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    //Debug.Log(_canDoubleJump);
                }

                if (_isWalled && !_isGrounded) 
                {
                    _rb2d.velocity = Vector2.zero;
                    Vector2 jumping = Vector2.up * buttonValue * (_jumpForce / 2);
                    //Debug.Log(jumping);
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                    _canDoubleJump = true;
                    //Debug.Log(_canDoubleJump);
                }
            }
        }

        private void ActiveSpecialSpike()
        {
            _playerData.SpecialSpike(gameObject, _ball, _move);
            
            _hasTheBall = false;
            _isSpecialSpike = false;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        private void ReverseHaveTheBall()
        {
            _hasTheBall = !_hasTheBall;
        }

        private void ReverseCanPerfectReception()
        {
            _canPerfectReception = !_canPerfectReception;
        }
    }
}