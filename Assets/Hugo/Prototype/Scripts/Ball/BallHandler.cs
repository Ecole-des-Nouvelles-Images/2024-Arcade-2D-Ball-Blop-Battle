using System;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Ball
{
    public class BallHandler : MonoBehaviour
    {
        public Vector2 DirectionCommitment;
        
        private Rigidbody2D _rb2d;
        private Collider2D _col2D;
        private SpriteRenderer _sr;
        
        private GameObject _playerObject;
        
        private bool _isCatch;
        
        // Specal Spike
        private bool _isTransparent;
        private bool _canHitAgain;

        // Ball Settings
        [Header("Ball Settings")]
        [SerializeField] private float _speedDrawn;
        [SerializeField] private float _speedPunch;
        [SerializeField] private float _speedPerfectReception;
        [SerializeField] private float _speedSpecialSpikeActivation;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotationFactor;
        [SerializeField] private float _maxRotationSpeed;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _col2D = GetComponent<Collider2D>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            //Debug.Log(DirectionCommitment);
            // Engagement
            Commitment(DirectionCommitment);
        }

        private void Update()
        {
            // Pour que le ballon reste dans le joueur
            if (_isCatch)
            {
                transform.position = _playerObject.transform.position;
            }
            
            // Détruit le ballon a la fin du temps
            if (MatchManager.IsSetOver)
            {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            // Clamp la vitesse du ballon
            if (_rb2d.velocity.magnitude > _maxSpeed)
            {
                _rb2d.velocity = _rb2d.velocity.normalized * (_maxSpeed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("PlayerOneGround"))
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
                Destroy(gameObject);
            }
            if (other.gameObject.CompareTag("PlayerTwoGround"))
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                Destroy(gameObject);
            }
            
            if (other.gameObject.CompareTag("Player"))
            {
                _playerObject = other.gameObject;
                if (_isTransparent)
                {
                    YellowSpecialSpikeTransparent();
                }
                
                // Disable Green Spacial Spike
                _playerObject.GetComponent<PlayerController>().GreenSpecialSpike = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("AboveNet"))
            {
                if (_playerObject != null)
                {
                    _playerObject.GetComponent<PlayerNumberTouchBallManager>().NumberTouchBall = 0;
                    Debug.Log(" CROSS NET ");
                }
            }
        }

        private void OnDestroy()
        {
            if (_playerObject != null)
            {
                _playerObject.GetComponent<PlayerNumberTouchBallManager>().NumberTouchBall = 0;
            }

            if (!MatchManager.IsSetOver)
            {
                MatchManager.IsBallInGame = false;
            }
            
            // Disable Green Spacial Spike
            _playerObject.GetComponent<PlayerController>().GreenSpecialSpike = false;
        }

        private void Commitment(Vector2 direction)
        {
            _rb2d.AddForce(direction, ForceMode2D.Impulse);
        }

        public void IsAbsorb(GameObject playerObject)
        {
            _playerObject = playerObject;
            
            _col2D.isTrigger = true;
            _isCatch = true;
            
            _rb2d.velocity = Vector2.zero;
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public void IsShoot(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(Vector2.up * _speedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ReverseIsTrigger), 0.1f);
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(direction * _speedDrawn, ForceMode2D.Impulse);
                Invoke(nameof(ReverseIsTrigger), 0.1f);
            }
        }

        public void IsPunch(Vector2 direction, Vector2 playerVelocity)
        {
            Vector2 velocity = new Vector2(direction.x * playerVelocity.y * _speedPunch, direction.y * playerVelocity.y * _speedPunch);
            _rb2d.AddForce(velocity, ForceMode2D.Impulse);
            //Debug.Log(_rb2d.velocity.magnitude);
        }

        public void PerfectReception()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfectReception / 10, ForceMode2D.Impulse);
        }

        public void SpecialSpikeActivation()
        {
            _rb2d.velocity /= 2;
            _rb2d.AddForce(Vector2.up * _speedSpecialSpikeActivation / 10, ForceMode2D.Impulse);
        }
        
        // Special Spike
        // Jaune
        public void YellowSpecialSpikeTransparent()
        {
            _isTransparent = !_isTransparent;

            if (_isTransparent)
            {
                _sr.color = new Color(1,1,1, 0.2f);
            }
            else
            {
                _sr.color = new Color(1, 1, 1, 1);
            }
        }
        
        // Vert
        public void GreenSpecialSpikeHitAgain()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.down * _speedDrawn, ForceMode2D.Impulse);
        }
        
        
        // Utils
        public void InvokeMethodTimer([NotNull] string methodName, float timer)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Invoke(methodName, timer);
        }

        public void ReverseIsTrigger()
        {
            _col2D.isTrigger = !_col2D.isTrigger;
        }

        public void ReversIsCatch()
        {
            _isCatch = !_isCatch;
        }
    }
}
