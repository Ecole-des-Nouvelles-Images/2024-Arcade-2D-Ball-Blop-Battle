using UnityEngine;

namespace Hugo.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        
        //GameObject
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
        private float _isSouthButtonPressed;
        
        // Player Settings
        [Header("Player Settings")]
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _jumpForce;
        
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
            if (Mathf.Approximately(_isWestButtonPressed, 1) && _dashCooldownRemaining <= 0 && _hasTheBall == false)
            {
                _isDashing = true;
                _dashTimeRemaining = _dashDuration;
                _dashCooldownRemaining = _dashCooldown;
            }
            
            if (_isDashing)
            {
                _sr.color = Color.blue;
                
                _canMove = false;
                _rb2d.gravityScale = 0;

                if (_isGrounded)
                {
                    transform.Translate(_move.x * (_dashSpeed * Time.deltaTime), 0, 0);
                    _dashTimeRemaining -= Time.deltaTime;
                }
                else
                {
                    transform.Translate(_move * (_dashSpeed * Time.deltaTime));
                    _dashTimeRemaining -= Time.deltaTime;
                }
                
                if (_dashTimeRemaining <= 0)
                {
                    _isDashing = false;
                    _canMove = true;
                    _rb2d.gravityScale = 3;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_hasTheBall)
            {
                _sr.color = new Color(1, 1, 1, 1f);
                
                var horizontalInput = _move.x;
                
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

                Vector2 movement = new Vector2(horizontalInput * (_speed * Time.deltaTime), _rb2d.velocity.y);
                _rb2d.velocity = movement;
            }
            else
            {
                _sr.color = new Color(1, 1, 1, 0.5f);
                
                _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
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
        
        public void GetWestButtonReadValue(float isButtonPressed)
        {
            _isWestButtonPressed = isButtonPressed;
            //Debug.Log(_isWestButtonPressed);

            if (!_hasTheBall)
            {
                //Debug.Log("Je peux bouger");
                // Dash
                
            }
            else
            {
                
            }
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
            else
            {
                Debug.Log(" Tir ! " + buttonValue);
                if (Mathf.Approximately(buttonValue, 1))
                {
                    _ball.GetComponent<BallHandler>().IsDrawn(_move);
                    Invoke(nameof(DontHaveTheBallAnymore), 0.1f);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball") && !_isGrounded)
            {
                _ball = other.gameObject;
                _ball.GetComponent<BallHandler>().IsCatch(gameObject);
                _hasTheBall = true;
                _isDashing = false;
                _canMove = true;
                _rb2d.gravityScale = 3;
                Debug.Log(_ball);
            }
        }

        private void DontHaveTheBallAnymore()
        {
            _hasTheBall = false;
        }
    }
}