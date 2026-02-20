using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class BallController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2d;
        [SerializeField] private Collider2D _col2D;
        
        [Header("Ball Settings")]
        [SerializeField] private float _speedDrawn;
        [SerializeField] private float _speedPunch;
        [SerializeField] private float _speedPerfectReception;
        [SerializeField] private float _speedDashReception;
        [SerializeField] private float _speedSpecialSpikeActivation;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotationFactor;
        [SerializeField] private float _maxRotationSpeed;
        
        [Header("States")]
        [SerializeField] private bool _isAbsorbed;
        
        private void FixedUpdate()
        {
            // MAX SPEED
            if (_rb2d.velocity.magnitude > _maxSpeed)
            {
                _rb2d.velocity = _rb2d.velocity.normalized * (_maxSpeed * Time.deltaTime);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Selling"))
            {
                Vector2 direction = _rb2d.velocity;
                Vector2 newDirection = direction;
                newDirection.x = -_rb2d.velocity.x;
                _rb2d.velocity = newDirection;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _col2D.isTrigger = false;
            }
        }
        
        public void PerfectReception()
        {
            Debug.Log("Perfect Reception");
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfectReception, ForceMode2D.Impulse);
        }
        
        public void DashReception()
        {
            Debug.Log("Dash Reception");
            _rb2d.velocity /= 5;
            _rb2d.AddForce(Vector2.up * _speedDashReception, ForceMode2D.Impulse);
        }

        public void Absorb(Transform t)
        {
            _isAbsorbed = true;

            transform.position = t.position;
            _col2D.isTrigger = true;
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void Drawn(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isAbsorbed = false;
                
                _rb2d.AddForce(Vector2.up * _speedDrawn, ForceMode2D.Impulse);
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isAbsorbed = false;
                
                _rb2d.AddForce(direction * _speedDrawn, ForceMode2D.Impulse);
            }
        }
    }
}
