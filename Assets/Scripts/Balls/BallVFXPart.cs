using UnityEngine;

namespace Balls
{
    public class BallVFXPart : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _delayDetection = 0.1f;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _psAppears;
        [SerializeField] private GameObject _psDisappears;
        [SerializeField] private GameObject _psImpact;

        private float _spawnTime;

        private void Awake()
        {
            Instantiate(_psAppears, transform.position, Quaternion.identity);
        }

        private void Start()
        {
            _spawnTime = Time.time;
        }
        
        private void OnDisable()
        {
            Instantiate(_psDisappears, transform.position, Quaternion.identity);
        }
        
        private void Update()
        {
            if (Time.time < _spawnTime + _delayDetection) return;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.55f);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") || hit.CompareTag("Wall") || hit.CompareTag("Selling") 
                    || hit.CompareTag("PlayerOneGround") || hit.CompareTag("PlayerTwoGround"))
                {
                    Instantiate(_psImpact, transform.position, Quaternion.identity);
                    
                    _spawnTime = Time.time;
                }
            }
        }
    }
}