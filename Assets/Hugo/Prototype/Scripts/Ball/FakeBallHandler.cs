using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Ball
{
    public class FakeBallHandler : MonoBehaviour
    {
        // Components
        private Rigidbody2D _rb2d;
        private Collider2D _col2d;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _col2d = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerOneGround") || other.gameObject.CompareTag("PlayerTwoGround"))
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            // Touner le ballon dans sa direction
            Vector2 direction = _rb2d.velocity;
            if (direction.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public void Setup(Vector2 direction, float speed)
        {
            _rb2d.AddForce(direction * speed, ForceMode2D.Impulse);
        }
        
        // Utils
        public void InvokeMethodTimer([NotNull] string methodName, float timer)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Invoke(methodName, timer);
        }

        private void ReverseIsTrigger()
        {
            _col2d.isTrigger = !_col2d.isTrigger;
        }
    }
}
