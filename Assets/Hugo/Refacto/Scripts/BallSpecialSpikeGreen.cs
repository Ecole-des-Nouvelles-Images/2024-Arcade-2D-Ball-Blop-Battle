using Int.Scripts.Utils;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class BallSpecialSpikeGreen : MonoBehaviour
    {
        public void SecondHit(float speed)
        {
            Rigidbody2D rb2d = GetComponentInParent<Rigidbody2D>();
            rb2d.velocity = new Vector2(0f, -speed);
            
            Destroy(gameObject);
        }
        
        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
        }

        private void SpecialSpikeActivated()
        {
            Destroy(gameObject);
        }
        
        private void OnDisable()
        {
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
        }

        #endregion
    }
}
