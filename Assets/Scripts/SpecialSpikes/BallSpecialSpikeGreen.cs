using UnityEngine;
using Utils;

namespace SpecialSpikes
{
    public class BallSpecialSpikeGreen : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _delayDetection = 0.2f;
        
        [Header("Debug")]
        [SerializeField] private PlayerSpecialSpikeGreen _playerSpecialSpikeGreen;

        private float _spawnTime;
        
        private void Start()
        {
            _spawnTime = Time.time;
        }

        private void Update()
        {
            if (Time.time < _spawnTime + _delayDetection) return;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    Destroy(_playerSpecialSpikeGreen.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        public void Setup(PlayerSpecialSpikeGreen playerSpecialSpikeGreen)
        {
            _playerSpecialSpikeGreen = playerSpecialSpikeGreen;
        }
        
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
