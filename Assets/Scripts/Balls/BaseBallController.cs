using UnityEngine;

namespace Balls
{
    public abstract class BaseBallController : MonoBehaviour, IBall
    {
        [Header("===== BASE BALL SETTINGS =====")]
        [Header("== REFERENCES ==")]
        [SerializeField] protected Rigidbody2D _rb2d;
        [SerializeField] protected Collider2D _col2D;
        
        [Header("== BALL ==")]
        [SerializeField] protected float _speedDrawn = 2.3f;
        [SerializeField] protected float _speedPunch = 0.03f;
        [SerializeField] protected float _speedPerfectReception = 1f;
        [SerializeField] protected float _speedDashReception = 0.7f;
        [SerializeField] protected float _speedSpecialSpikeActivation = 0.3f;
        [SerializeField] protected float _maxSpeed = 50f;
        [SerializeField] protected float _rotationFactor = 10f;
        [SerializeField] protected float _maxRotationSpeed = 20f;

        [Header("== STATES ==")]
        [SerializeField] protected bool _isAbsorbed;

        protected Transform _playerTransform;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        protected virtual void Update()
        {
            if (_isAbsorbed && _playerTransform)
            {
                transform.position = _playerTransform.position;
            }
        }

        protected virtual void FixedUpdate()
        {
            if (_rb2d.velocity.magnitude > _maxSpeed)
            {
                _rb2d.velocity = _rb2d.velocity.normalized * _maxSpeed;
            }
        }

        public virtual void PerfectReception()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfectReception, ForceMode2D.Impulse);
        }
        
        public virtual void DashReception()
        {
            _rb2d.velocity /= 5f;
            _rb2d.AddForce(Vector2.up * _speedDashReception, ForceMode2D.Impulse);
        }

        public virtual void Absorb(Transform playerTransform)
        {
            _isAbsorbed = true;
            _playerTransform = playerTransform;
            _col2D.isTrigger = true;
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public virtual void Drawn(Vector2 direction)
        {
            _isAbsorbed = false;
            _playerTransform = null;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

            Vector2 forceDir = direction == Vector2.zero ? Vector2.up : direction;
            _rb2d.AddForce(forceDir * _speedDrawn, ForceMode2D.Impulse);
        }

        public virtual void DrawnSpecialSpike(Vector2 direction, float speed)
        {
            _isAbsorbed = false;
            _playerTransform = null;
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

            Vector2 forceDir = direction == Vector2.zero ? Vector2.up : direction;
            _rb2d.AddForce(forceDir * speed, ForceMode2D.Impulse);
        }
    }
}