using UnityEngine;

namespace Hugo.Scripts
{
    public class BallHandler : MonoBehaviour
    {
        private Collider2D _collider2D;
        
        private GameObject _playerObject;
        
        private bool _isCatch;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _playerObject = other.gameObject;
                other.gameObject.GetComponent<PlayerController>().HasTheBall = true;
                _collider2D.isTrigger = true;
                _isCatch = true;
            }
        }

        private void Update()
        {
            if (_isCatch)
            {
                transform.position = _playerObject.transform.position;
            }
        }
    }
}
