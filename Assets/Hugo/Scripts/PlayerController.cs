using UnityEngine;

namespace Hugo.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        
        // GameObject
        private GameObject _ball;
        
        // States
        private bool _hasTheBall;
        private bool _isDashing;
        private bool _canMove = true;
        private bool _isGrounded;
        private bool _isWalled;
        
        // Inputs values
        private Vector2 _move;
        private float _isWestButtonPressed;
        private float _isEastButtonPressed;
        private float _isSouthButtonPressed;
        
        // Player Settings
        [Header("Player Settings")]
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _jumpForce;
        [SerializeField]
        private float _jumpingSpeed;
        
        // Dash Settings
        [Header("Dash Settings")]
        [SerializeField]
        private float _dashSpeed;
        [SerializeField]
        private float _dashDuration;
        [SerializeField]
        private float _dashCooldown;
        private float _dashTimeRemaining;
        private float _dashCooldownRemaining;
        
        // isGrounded and isWalled
        [Header("Is Grounded and Is Walled")]
        [SerializeField]
        private float _rayLength = 0.8f;
        [SerializeField]
        private LayerMask _groundLayer;
        [SerializeField]
        private LayerMask _wallLayer;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            // Raycast _isGrounded
            RaycastHit2D hit2DGround = Physics2D.Raycast(transform.position, Vector3.down, _rayLength, _groundLayer);
            _isGrounded = hit2DGround.collider;
            
            Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.red);

            _isWalled = false;
            if (-1 <= _move.x && _move.x <= -0.8 || 0.8 <= _move.x && _move.x <= 1)
            {
                // Raycast _isWalled
                RaycastHit2D hit2DWallRight = Physics2D.Raycast(transform.position, Vector3.right, _rayLength, _wallLayer);
                RaycastHit2D hit2DWallLeft = Physics2D.Raycast(transform.position, Vector3.left, _rayLength, _wallLayer);
                if (hit2DWallLeft || hit2DWallRight)
                {
                    _isWalled = true;
                }
                
                Debug.DrawRay(transform.position, Vector3.right * _rayLength, Color.red);
                Debug.DrawRay(transform.position, Vector3.left * _rayLength, Color.red);
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
            if (Mathf.Approximately(_isEastButtonPressed, 1) && _dashCooldownRemaining <= 0 && _hasTheBall == false && _isGrounded && _move != Vector2.zero)
            {
                _isDashing = true;
                _dashTimeRemaining = _dashDuration;
                _dashCooldownRemaining = _dashCooldown;
            }
            
            if (_isDashing)
            {
                _sr.color = Color.blue;
                
                _canMove = false;

                transform.Translate(_move.x * (_dashSpeed * Time.deltaTime), 0, 0);
                _dashTimeRemaining -= Time.deltaTime;
                
                if (_dashTimeRemaining <= 0)
                {
                    _isDashing = false;
                    _canMove = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_hasTheBall)
            {
                _sr.color = new Color(1, 1, 1, 1f);
                
                var horizontalInput = _move.x;
                
                Vector2 movement = new Vector2(horizontalInput * (_speed * Time.deltaTime), _rb2d.velocity.y);
                _rb2d.velocity = movement;
            }
            else
            {
                _sr.color = new Color(1, 1, 1, 0.5f);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ball = other.gameObject;
                if (Mathf.Approximately(_isWestButtonPressed, 1))
                {
                    if (_isGrounded)
                    {
                        if (_move == Vector2.zero)
                        {
                            _ball.GetComponent<BallHandler>().PerfectReception();
                            Debug.Log(" Perfect reception ! ");
                        }
                    }
                    else
                    {
                        _ball.GetComponent<BallHandler>().IsCatch(gameObject);
                        _hasTheBall = true;
                        _isDashing = false;
                        _canMove = true;
                    }
                }
                else
                {
                    Vector2 direction = new Vector2(_ball.transform.position.x - transform.position.x, _ball.transform.position.y - transform.position.y);
                    _ball.GetComponent<BallHandler>().IsPunch(direction, _rb2d.velocity);
                }
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
            Debug.Log(_isWestButtonPressed);

            if (buttonValue == 0 && _hasTheBall)
            {
                _ball.GetComponent<BallHandler>().IsDrawn(_move);
                Invoke(nameof(DontHaveTheBallAnymore), 0.1f);
            }
        }

        public void GetEastButtonReadValue(float buttonValue)
        {
            _isEastButtonPressed = buttonValue;
            //Debug.Log(_isEastButtonPressed);
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            _isSouthButtonPressed = buttonValue;
            //Debug.Log(_isSouthButtonPressed);

            if (!_hasTheBall)
            {
                if (_isGrounded && !_isWalled || _isGrounded && _isWalled)
                {
                    Vector2 jumping = Vector2.up * buttonValue * _jumpForce;
                    //Debug.Log(jumping);
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                }

                if (_isWalled && !_isGrounded)
                {
                    Vector2 jumping = Vector2.up * buttonValue * (_jumpForce / 2);
                    //Debug.Log(jumping);
            
                    _rb2d.AddForce(jumping, ForceMode2D.Impulse);
                }
            }
        }

        private void DontHaveTheBallAnymore()
        {
            _hasTheBall = false;
        }
    }
}