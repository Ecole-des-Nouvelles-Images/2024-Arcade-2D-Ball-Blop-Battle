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
        private float _ballSpeedDrawn;
        [SerializeField]
        private float _ballSpeedPunch;
        [SerializeField]
        private float _speedPerfetcReception;

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
            if (direction == Vector2.zero)
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(Vector2.up * _ballSpeedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ChangeIsTrigger), 0.1f);
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(direction * _ballSpeedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ChangeIsTrigger), 0.1f);
            }
        }

        public void IsPunch(Vector2 direction, Vector2 playerVelocity)
        {
            //_rb2d.velocity /= 2;
            _rb2d.AddForce(direction * playerVelocity * _ballSpeedPunch, ForceMode2D.Impulse);
        }

        public void PerfectReception()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfetcReception / 10, ForceMode2D.Impulse);
        }

        private void ChangeIsTrigger()
        {
            _col2D.isTrigger = false;
        }
    }
}
