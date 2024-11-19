using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private SpriteRenderer _sr;
        
        // Player Settings
        [FormerlySerializedAs("_hasTheBall")] [Header("Player Settings")]
        public bool HasTheBall;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _jumpForce;

        // Inputs values
        private Vector2 _move;
        private float _isWestButtonPressed;
        private float _isSouthButtonPressed;
        
        // isGrounded and isWalled
        [Header("Is grounded and Is walled")]
        [SerializeField]
        private float _rayLength = 0.8f;
        [SerializeField]
        private LayerMask _groundLayer;
        [SerializeField]
        private LayerMask _wallLayer;
        private bool _isGrounded;
        private bool _isWalled;
        
        // isGrounded and isWalled
        [Header("Ball Settings")]
        [SerializeField]
        private float _ballSpeed;
        

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
                
                // Debug.DrawRay(transform.position, Vector3.right * _rayLength, Color.red);
                // Debug.DrawRay(transform.position, Vector3.left * _rayLength, Color.red);
            }
            
            // Debug.DrawRay(transform.position, Vector3.down * _rayLength, Color.red);
            // Debug.Log(_isGrounded);
            // Debug.Log(_isWalled);
        }

        private void FixedUpdate()
        {
            if (!HasTheBall)
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
                
                Vector2 aim = _move;
                
                _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public void GetJoystickReadValue(Vector2 move)
        {
            _move = move;
            Debug.Log(_move);
        }
        
        public void GetWestButtonReadValue(float isButtonPressed)
        {
            _isWestButtonPressed = isButtonPressed;
            //Debug.Log(_isWestButtonPressed);

            if (!HasTheBall)
            {
                
            }
            else
            {
                
            }
        }
        
        public void GetSouthButtonReadValue(float buttonValue)
        {
            _isSouthButtonPressed = buttonValue;
            //Debug.Log(_isSouthButtonPressed);

            if (!HasTheBall)
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
                
            }
        }
    }
}