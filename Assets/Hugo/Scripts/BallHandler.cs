using UnityEngine;

namespace Hugo.Scripts
{
    public class BallHandler : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private Collider2D _col2D;
        
        private GameObject _playerObject;
        
        private bool _isCatch;

        [Header("Ball Settings")]
        [SerializeField]
        private float _speedDrawn;
        [SerializeField]
        private float _speedPunch;
        [SerializeField]
        private float _speedPerfectReception;
        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        private float _rotationFactor;
        [SerializeField]
        private float _maxRotationSpeed;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _col2D = GetComponent<Collider2D>();
        }

        private void Start()
        {
            _rb2d.AddForce(new Vector2(1,1), ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (_isCatch)
            {
                transform.position = _playerObject.transform.position;
                //Debug.Log(_rb2d.velocity.magnitude);
            }
            
            // Ball Rotation
            // float rotation = Math.Clamp(_rb2d.velocity.x * _rotationFactor, -_maxRotationSpeed, _maxRotationSpeed * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(0f, 0f, -rotation);
        }

        private void FixedUpdate()
        {
            if (_rb2d.velocity.magnitude > _maxSpeed)
            {
                _rb2d.velocity = _rb2d.velocity.normalized * (_maxSpeed * Time.deltaTime);
            }
        }

        public void IsCatch(GameObject playerObject)
        {
            _playerObject = playerObject;
            
            _col2D.isTrigger = true;
            _isCatch = true;
            
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void IsDrawn(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(Vector2.up * _speedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ChangeIsTrigger), 0.1f);
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(direction * _speedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ChangeIsTrigger), 0.1f);
            }
        }

        public void IsPunch(Vector2 direction, Vector2 playerVelocity)
        {
            Vector2 velocity = new Vector2(direction.x * playerVelocity.y * _speedPunch, direction.y * playerVelocity.y * _speedPunch);
            _rb2d.AddForce(velocity, ForceMode2D.Impulse);
            //Debug.Log(_rb2d.velocity.magnitude);
        }

        public void PerfectReception()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfectReception / 10, ForceMode2D.Impulse);
        }

        private void ChangeIsTrigger()
        {
            _col2D.isTrigger = false;
        }
    }
}
