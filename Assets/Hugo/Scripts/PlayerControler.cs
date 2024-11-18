using UnityEngine;

namespace Hugo.Scripts
{
    public class PlayerControler : MonoBehaviour
    {
        private Rigidbody2D _rb2d;
        [SerializeField] private bool _hasTheBall;

        public float Speed;
        public float JumpForce;
        
        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            bool jump = Input.GetButtonDown("Fire1");
            bool freeze = Input.GetButtonDown("Fire2");

            if (!_hasTheBall)
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                
                Vector2 movement = new Vector2(horizontalInput * (Speed * Time.deltaTime), _rb2d.velocity.y);
                _rb2d.velocity = movement;

                if (jump)
                {
                    _rb2d.AddForce(JumpForce * Vector2.up, ForceMode2D.Impulse);
                }
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            if (freeze)
            {
                _hasTheBall = !_hasTheBall;
            }
        }
    }
}
