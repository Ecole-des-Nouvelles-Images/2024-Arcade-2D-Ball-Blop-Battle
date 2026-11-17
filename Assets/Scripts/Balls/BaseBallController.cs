using System.Collections.Generic;
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

        [Header("== THROWER COLLISION ==")]
        [Tooltip("Durée max (s) pendant laquelle la balle ignore le blop qui vient de la lancer.")]
        [SerializeField] protected float _ignoreThrowerMaxDuration = 0.5f;
        [Tooltip("Distance (unités) à partir de laquelle la balle est considérée séparée du lanceur.")]
        [SerializeField] protected float _throwerSeparationDistance = 0.05f;

        [Header("== STATES ==")]
        [SerializeField] protected bool _isAbsorbed;

        protected Transform _playerTransform;

        // Colliders du lanceur ignorés juste après un tir : la balle reste solide pour les murs / plafond,
        // mais ne se coince pas dans le blop qui la tenait.
        private readonly List<Collider2D> _ignoredThrowerColliders = new();
        private readonly List<Collider2D> _colliderBuffer = new();
        private float _ignoreThrowerEndTime;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public Rigidbody2D Rigidbody => _rb2d;
        public Collider2D Collider => _col2D;
        public bool IsAbsorbed => _isAbsorbed;
        public float DrawnImpulse => _speedDrawn;

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

            UpdateIgnoredThrower();
        }

        protected virtual void OnDestroy()
        {
            RestoreThrowerCollisions();
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
            RestoreThrowerCollisions();

            _isAbsorbed = true;
            _playerTransform = playerTransform;
            _col2D.isTrigger = true;
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public virtual void Drawn(Vector2 direction)
        {
            Launch(direction, _speedDrawn);
        }

        public virtual void DrawnSpecialSpike(Vector2 direction, float speed)
        {
            Launch(direction, speed);
        }

        /// <summary>Lâche la balle sans impulsion (mort du porteur, faute...).</summary>
        public virtual void Release()
        {
            if (!_isAbsorbed) return;

            Transform thrower = _playerTransform;
            Unabsorb();
            IgnoreThrower(thrower);
            _rb2d.velocity = Vector2.zero;
        }

        private void Launch(Vector2 direction, float impulse)
        {
            Transform thrower = _playerTransform;
            Unabsorb();
            IgnoreThrower(thrower);

            Vector2 forceDir = direction == Vector2.zero ? Vector2.up : direction;
            _rb2d.AddForce(forceDir * impulse, ForceMode2D.Impulse);
        }

        private void Unabsorb()
        {
            _isAbsorbed = false;
            _playerTransform = null;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        #region === THROWER COLLISION ===

        /// <summary>
        /// Rend la balle solide tout en ignorant les colliders du lanceur jusqu'à ce qu'elle en soit sortie.
        /// Sans ça, la balle restait trigger après un tir et traversait murs et plafond.
        /// </summary>
        protected void IgnoreThrower(Transform thrower)
        {
            RestoreThrowerCollisions();
            _col2D.isTrigger = false;

            if (!thrower) return;

            thrower.GetComponentsInChildren(false, _colliderBuffer);
            foreach (Collider2D col in _colliderBuffer)
            {
                if (!col || col.isTrigger || col == _col2D) continue;

                Physics2D.IgnoreCollision(_col2D, col, true);
                _ignoredThrowerColliders.Add(col);
            }
            _colliderBuffer.Clear();

            _ignoreThrowerEndTime = Time.time + _ignoreThrowerMaxDuration;
        }

        private void UpdateIgnoredThrower()
        {
            if (_ignoredThrowerColliders.Count == 0) return;

            if (Time.time < _ignoreThrowerEndTime)
            {
                foreach (Collider2D col in _ignoredThrowerColliders)
                {
                    if (!col || !col.isActiveAndEnabled) continue;

                    ColliderDistance2D distance = _col2D.Distance(col);
                    if (distance.isValid && distance.distance < _throwerSeparationDistance) return;
                }
            }

            RestoreThrowerCollisions();
        }

        private void RestoreThrowerCollisions()
        {
            foreach (Collider2D col in _ignoredThrowerColliders)
            {
                if (col && _col2D) Physics2D.IgnoreCollision(_col2D, col, false);
            }
            _ignoredThrowerColliders.Clear();
        }

        #endregion
    }
}
