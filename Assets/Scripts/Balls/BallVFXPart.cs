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
        private readonly Collider2D[] _results = new Collider2D[8];

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
            
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, 0.55f, _results);

            for (int i = 0; i < hitCount; i++)
            {
                if (_results[i].CompareTag("Player") || _results[i].CompareTag("Wall") || _results[i].CompareTag("Selling") 
                    || _results[i].CompareTag("PlayerOneGround") || _results[i].CompareTag("PlayerTwoGround"))
                {
                    Instantiate(_psImpact, transform.position, Quaternion.identity);
                    
                    _spawnTime = Time.time;
                    break;
                }
            }
        }
    }
}