using UnityEngine;

namespace Balls
{
    public class FakeBallController : BaseBallController
    {
        protected override void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") || 
                other.gameObject.CompareTag("PlayerOneGround") || 
                other.gameObject.CompareTag("PlayerTwoGround"))
            {
                Destroy(gameObject);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerOneGround") || 
                other.gameObject.CompareTag("PlayerTwoGround"))
            {
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Selling"))
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

        public void Setup(Vector2 direction, float speed)
        {
            if (_rb2d != null)
            {
                _rb2d.AddForce(direction * speed, ForceMode2D.Impulse);
            }
        }
        
        public override void PerfectReception()
        {
            Destroy(gameObject);
        }
        
        public override void DashReception()
        {
            Destroy(gameObject);
        }

        public override void Absorb(Transform playerTransform)
        {
            Destroy(gameObject);
        }

        public override void Drawn(Vector2 direction)
        {
            Destroy(gameObject);
        }

        public override void DrawnSpecialSpike(Vector2 direction, float speed)
        {
            Destroy(gameObject);
        }
    }
}