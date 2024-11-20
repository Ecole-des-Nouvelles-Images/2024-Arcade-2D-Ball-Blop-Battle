using UnityEngine;

namespace Hugo.Scripts
{
    public class BallHandler : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        private Collider2D _col2D;
        
        private GameObject _playerObject;
        
        private bool _isCatch;

        [SerializeField]
        private float _ballSpeed;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _col2D = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (_isCatch)
            {
                transform.position = _playerObject.transform.position;
                //Debug.Log(_rb2d.velocity.magnitude);
            }
        }

        public void IsCatch(GameObject playerObject)
        {
            _playerObject = playerObject;
            
            _col2D.isTrigger = true;
            _isCatch = true;
            
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void IsDrawn(Vector2 direction)
        {
            Debug.Log(direction);
            
            _rb2d.constraints = RigidbodyConstraints2D.None;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            _isCatch = false;
                
            _rb2d.AddForce(direction * _ballSpeed, ForceMode2D.Impulse);
            Invoke(nameof(ChangeIsTrigger), 0.1f);
        }

        private void ChangeIsTrigger()
        {
            _col2D.isTrigger = false;
        }
    }
}
