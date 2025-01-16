using System;
using Hugo.Prototype.Scripts.Camera;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using Hugo.Prototype.Scripts.Sounds;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.Prototype.Scripts.Ball
{
    public class BallHandler : MonoBehaviour
    {
        public Vector2 DirectionCommitment;
        public bool IsPlayerOneSide;
        
        private Rigidbody2D _rb2d;
        private Collider2D _col2D;
        private SpriteRenderer _sr;
        private Transform _transform;
        private MatchManager _matchManager;
        private CameraHandler _cameraHandler;
        private AudioSource _audioSource;
        
        private GameObject _currentPlayerGameObject;
        private GameObject _lastPlayerGameObject;
        
        private bool _isCatch;
        private bool _canCommitment;
        
        // Special Spike
        private bool _isTransparent;
        private bool _isSmaller;

        // Ball Settings
        [Header("Ball Settings")]
        [SerializeField] private float _speedDrawn;
        [SerializeField] private float _speedPunch;
        [SerializeField] private float _speedPerfectReception;
        [SerializeField] private float _speedSpecialSpikeActivation;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotationFactor;
        [SerializeField] private float _maxRotationSpeed;
        
        [Header("   Particle System")]
        public GameObject BallBaseTrail;
        [SerializeField] private ParticleSystem _ballAppearsDisappears;
        public ParticleSystem BallDisappears;
        [SerializeField] private ParticleSystem _ballHits;
        [Header("Blue")]
        public GameObject VFXSpecialSpikeBlue;
        public ParticleSystem VFXSpecialSpikeBlueImpact;
        [Header("Yellow")]
        public GameObject VFXSpecialSpikeYellow;
        public ParticleSystem VFXSpecialSpikeYellowImpact;
        [SerializeField] private ParticleSystem _specialSpikeYellowImpact;
        [Header("Red")]
        public GameObject VFXSpecialSpikeRed;
        [Header("Green")]
        public GameObject VFXSpecialSpikeGreen;
        public ParticleSystem VFXSpecialSpikeGreenImpact;
        
        [Header("Celebrations")]
        [SerializeField] private GameObject _celebrationPointObject;

        private void Awake()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _col2D = GetComponent<Collider2D>();
            _sr = GetComponent<SpriteRenderer>();
            _transform = GetComponent<Transform>();
            _audioSource = GetComponent<AudioSource>();
            _matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
            _cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();
        }

        private void Start()
        {
            transform.position = DirectionCommitment;
            _sr.color = new Color(1f, 1f, 1f, 0f);
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            _col2D.enabled = false;
            
            // VFX
            _ballAppearsDisappears.Play();
            _canCommitment = false;
            Invoke(nameof(ReverseCanCommitment), 0.6f);
        }

        private void Update()
        {
            // Commitment
            if (_canCommitment)
            {
                _col2D.enabled = true;
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _rb2d.gravityScale = 0f;
                _sr.color = new Color(1f, 1f, 1f, 1f);
                Commitment(DirectionCommitment);
                _canCommitment = false;
            }
            
            // Touner le ballon dans sa direction
            Vector2 direction = _rb2d.velocity;
            if (direction.magnitude > 0.1f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            
            // Pour que le ballon reste dans le joueur
            if (_isCatch)
            {
                transform.position = _currentPlayerGameObject.transform.position;
                // Debug.Log("Is Catch");
            }
            
            // Détruit le ballon a la fin du temps
            if (MatchManager.IsSetOver)
            {
                Destroy();
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
                _matchManager.DisplayScoreChange(false, false);
                ResetVFXSpecialSpike();
                Instantiate(_celebrationPointObject, new Vector3(8, 2, 0), Quaternion.identity);
                if (PlayerController.ScrollBackgroundObjects.Count > 0)
                {
                    foreach (GameObject scroll in PlayerController.ScrollBackgroundObjects)
                    {
                        Destroy(scroll);
                    }
                }
                
                if (_lastPlayerGameObject)
                {
                    _lastPlayerGameObject.GetComponent<PlayerController>().ResetStatesAfterSpecialSpike();

                    // if (_lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.isPlaying)
                    // {
                    //     _lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.Stop();
                    // }
                }
                if (_currentPlayerGameObject)
                {
                    _currentPlayerGameObject.GetComponent<PlayerController>().ResetStatesAfterSpecialSpike();
                    
                    // if (_currentPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.isPlaying)
                    // {
                    //     _currentPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.Stop();
                    // }
                }
                _cameraHandler.ScoredShake();
                
                Destroy();
            }
            if (other.gameObject.CompareTag("PlayerTwoGround"))
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                _matchManager.DisplayScoreChange(true, false);
                ResetVFXSpecialSpike();
                Instantiate(_celebrationPointObject, new Vector3(-8, 2, 0), Quaternion.identity);
                if (PlayerController.ScrollBackgroundObjects.Count > 0)
                {
                    foreach (GameObject scroll in PlayerController.ScrollBackgroundObjects)
                    {
                        Destroy(scroll);
                    }
                }

                if (_lastPlayerGameObject)
                {
                    _lastPlayerGameObject.GetComponent<PlayerController>().ResetStatesAfterSpecialSpike();
                    
                    // if (_lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.isPlaying)
                    // {
                    //     _lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.Stop();
                    // }
                }
                if (_currentPlayerGameObject)
                {
                    _currentPlayerGameObject.GetComponent<PlayerController>().ResetStatesAfterSpecialSpike();
                    
                    // if (_currentPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.isPlaying)
                    // {
                    //     _currentPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.Stop();
                    // }
                }
                _cameraHandler.ScoredShake();

                Destroy();
            }
            
            if (other.gameObject.CompareTag("Player"))
            {
                if (_currentPlayerGameObject == null)
                {
                    _matchManager.IsTimerRunning = true;
                    _rb2d.gravityScale = 1f;
                    _currentPlayerGameObject = other.gameObject;
                }
                else if (_currentPlayerGameObject != other.gameObject)
                {
                    _lastPlayerGameObject = _currentPlayerGameObject;
                    _currentPlayerGameObject = other.gameObject;
                    
                    // Set NumberTouch of LastPlayer to 0
                    _lastPlayerGameObject.GetComponent<PlayerNumberTouchBallHandler>().NumberTouchBall = 0;
                    
                    // Disable Green Spacial Spike
                    _currentPlayerGameObject.GetComponent<PlayerController>().CountShootSpecialSpike = 0;
                }

                if (_lastPlayerGameObject)
                {
                    _lastPlayerGameObject.GetComponent<PlayerController>().CountShootSpecialSpike = 0;
                    
                    // if (_lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.isPlaying)
                    // {
                    //     _lastPlayerGameObject.GetComponent<PlayerController>().VfxShootSpecialSpikeRed.Stop();
                    // }
                }
                
                if (_isTransparent)
                {
                    YellowSpecialSpike();
                }

                if (_isSmaller)
                {
                    BlueSpecialSpike();
                }

                if (VFXSpecialSpikeYellow.activeSelf)
                {
                    _specialSpikeYellowImpact.Play();
                }

                if (PlayerController.ScrollBackgroundObjects.Count > 0)
                {
                    foreach (GameObject scroll in PlayerController.ScrollBackgroundObjects)
                    {
                        Destroy(scroll);
                    }
                }

                if (_currentPlayerGameObject.GetComponent<PlayerController>().PlayerType.PlayerName == "Bleu")
                {
                    BallBaseTrail.GetComponent<TrailRenderer>().startColor = new Color(0.25f, 0.57f, 0.75f);
                    BallBaseTrail.GetComponent<TrailRenderer>().endColor = new Color(0.25f, 0.57f, 0.75f);
                }
                if (_currentPlayerGameObject.GetComponent<PlayerController>().PlayerType.PlayerName == "Vert")
                {
                    BallBaseTrail.GetComponent<TrailRenderer>().startColor = new Color(0.4f, 0.75f, 0.25f);
                    BallBaseTrail.GetComponent<TrailRenderer>().endColor = new Color(0.4f, 0.75f, 0.25f);
                }
                if (_currentPlayerGameObject.GetComponent<PlayerController>().PlayerType.PlayerName == "Jaune")
                {
                    BallBaseTrail.GetComponent<TrailRenderer>().startColor = new Color(1f, 1f, 0f);
                    BallBaseTrail.GetComponent<TrailRenderer>().endColor = new Color(1f, 1f, 0f);
                }
                if (_currentPlayerGameObject.GetComponent<PlayerController>().PlayerType.PlayerName == "Rouge")
                {
                    BallBaseTrail.GetComponent<TrailRenderer>().startColor = new Color(0.88f, 0.3f, 0.23f);
                    BallBaseTrail.GetComponent<TrailRenderer>().endColor = new Color(0.88f, 0.3f, 0.23f);
                }
                
                ResetVFXSpecialSpike();
            }

            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Selling"))
            {
                // VFX
                _ballHits.Play();
                
                // SFX
                _audioSource.clip = AudioStock.Instance.BallClips[Random.Range(0, AudioStock.Instance.BallClips.Count)];
                _audioSource.Play();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                Vector2 direction = _rb2d.velocity;
                Vector2 newDirection = direction;
                newDirection.x = -_rb2d.velocity.x;
                _rb2d.velocity = newDirection;
                
                // Debug.Log(" Direction : " + direction + " New direction : " + newDirection);
            }
            
            if (other.gameObject.CompareTag("PlayerOneGround"))
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
                _matchManager.DisplayScoreChange(false, false);
                ResetVFXSpecialSpike();
                _cameraHandler.ScoredShake();
                Instantiate(_celebrationPointObject, new Vector3(8, 2, 0), Quaternion.identity);
                
                Destroy();
            }
            if (other.gameObject.CompareTag("PlayerTwoGround"))
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                _matchManager.DisplayScoreChange(true, false);
                ResetVFXSpecialSpike();
                _cameraHandler.ScoredShake();
                Instantiate(_celebrationPointObject, new Vector3(-8, 2, 0), Quaternion.identity);

                Destroy();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _col2D.isTrigger = false;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerOneSide"))
            {
                IsPlayerOneSide = true;
            }
            else if (other.gameObject.CompareTag("PlayerTwoSide"))
            {
                IsPlayerOneSide = false;
            }
        }

        private void OnDestroy()
        {
            if (!MatchManager.IsSetOver)
            {
                _matchManager.InvokeMethodTimer("Commitment");
                _matchManager.IsTimerRunning = false;
            }
            
            // Disable Green Spacial Spike
            if (_currentPlayerGameObject)
            {
                _currentPlayerGameObject.GetComponent<PlayerNumberTouchBallHandler>().NumberTouchBall = 0;
                _currentPlayerGameObject.GetComponent<PlayerController>().CountShootSpecialSpike = 0;

                if (_lastPlayerGameObject)
                {
                    _lastPlayerGameObject.GetComponent<PlayerController>().CountShootSpecialSpike = 0;
                }
            }
            
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            if (gameManager)
            {
                if (gameManager.GetComponent<GameManager>().FirstPlayerGameObject)
                {
                    if (gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().CanAbsorb)
                    {
                        gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
                        gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().IsSpecialSpike = false;
                        gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().CanAbsorb = false;
                    }

                    if (gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().ImpactBackgroundObject)
                    {
                        Destroy(gameManager.GetComponent<GameManager>().FirstPlayerGameObject.GetComponent<PlayerController>().ImpactBackgroundObject);
                    }
                }

                if (gameManager.GetComponent<GameManager>().SecondPlayerGameObject)
                {
                    if (gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().CanAbsorb)
                    {
                        gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
                        gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().IsSpecialSpike = false;
                        gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().CanAbsorb = false;
                    }

                    if (gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().ImpactBackgroundObject)
                    {
                        Destroy(gameManager.GetComponent<GameManager>().SecondPlayerGameObject.GetComponent<PlayerController>().ImpactBackgroundObject);
                    }
                }
            }
        }

        // Actions Player-Ball
        public void IsAbsorb(GameObject playerObject)
        {
            _currentPlayerGameObject = playerObject;
            
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
                
                _rb2d.AddForce(Vector2.up * _speedDrawn, ForceMode2D.Impulse);
            }
            else
            {
                _rb2d.constraints = RigidbodyConstraints2D.None;
                _rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
                _isCatch = false;
                
                _rb2d.AddForce(direction * _speedDrawn, ForceMode2D.Impulse);
            }
        }

        public void IsPunch(Vector2 direction, Vector2 playerVelocity)
        {
            Vector2 velocity = new Vector2(direction.x * playerVelocity.y * _speedPunch, direction.y * playerVelocity.y * _speedPunch);
            _rb2d.AddForce(velocity, ForceMode2D.Impulse);
        }

        public void PerfectReception()
        {
            _rb2d.velocity = Vector2.zero;
            _rb2d.AddForce(Vector2.up * _speedPerfectReception / 10, ForceMode2D.Impulse);
        }

        // Special Spike
        public void SpecialSpikeActivation()
        {
            _rb2d.velocity /= 3;
            _rb2d.AddForce(Vector2.up * _speedSpecialSpikeActivation / 10, ForceMode2D.Impulse);
        }
        
        // Jaune
        private void YellowSpecialSpike()
        {
            _isTransparent = !_isTransparent;

            _sr.color = _isTransparent ? new Color(1,1,1, 0f) : new Color(1, 1, 1, 1);
        }
        
        // Bleu
        private void BlueSpecialSpike()
        {
            _isSmaller = !_isSmaller;
            
            _transform.localScale = _isSmaller ? new Vector3(0.5f, 0.5f, 0.5f) : new Vector3(1f, 1f, 1f);
        }
        
        // Commitment
        private void Commitment(Vector2 direction)
        {
            transform.position = direction;
            
            // _rb2d.AddForce(direction, ForceMode2D.Impulse);
        }
        
        // Utils
        public void InvokeMethodTimer([NotNull] string methodName, float timer)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Invoke(methodName, timer);
        }

        public void ReversIsCatch()
        {
            _isCatch = !_isCatch;
        }

        private void ReverseCanCommitment()
        {
            _canCommitment = !_canCommitment;
        }

        private void ResetVFXSpecialSpike()
        {
            VFXSpecialSpikeBlue.SetActive(false);
            VFXSpecialSpikeGreen.SetActive(false);
            VFXSpecialSpikeRed.SetActive(false);
            VFXSpecialSpikeYellow.SetActive(false);
            
            BallBaseTrail.SetActive(true);
        }

        public void Destroy()
        {
            _rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            _col2D.enabled = false;
                
            // VFX
            BallDisappears.Play();
                
            _sr.color = new Color(1, 1, 1, 0);
            
            Destroy(gameObject, 0.3f);
        }
    }
}