using _Branches.Hugo.OldScripts.Ball;
using _Branches.Hugo.OldScripts.Game;
using _Branches.Hugo.OldScripts.Player;
using Cam;
using Sounds;
using UnityEngine;

namespace _Branches.Hugo.OldScripts.Laser
{
    public class OldLaserHandler : MonoBehaviour
    {
        [Header("Laser Settings")]
        [SerializeField] private float _rayLaserLength;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _ballLayer;
        [SerializeField] private ParticleSystem _laserParticles;
        
        [Header("Camera")]
        [SerializeField] private CameraHandler _cameraHandler;
        
        [Header("Celebrations")]
        [SerializeField] private GameObject _celebrationPointObject;

        private bool _hasAlreadyHit;
        private bool _isplayerOneHit;
        private GameObject _ballGameObject;
        private OldMatchManager _oldMatchManager;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            _oldMatchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<OldMatchManager>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            // Raycast _laserHit
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, Vector3.down, _rayLaserLength, _playerLayer);
            RaycastHit2D hitBall = Physics2D.Raycast(transform.position, Vector3.down, _rayLaserLength, _ballLayer);
            
            if (hitBall.collider)
            {
                _ballGameObject = hitBall.collider.gameObject;
            }
            
            if (hitPlayer.collider)
            {
                _oldMatchManager.IsTimerRunning = false;
                
                Debug.Log(" hit player ");
                GameObject playerChildren = hitPlayer.collider.gameObject;
                playerChildren.GetComponentInParent<OldPlayerController>().PlayerDie();
                _isplayerOneHit = playerChildren.GetComponentInParent<OldPlayerNumberTouchBallHandler>().IsPlayerOne;
                
                playerChildren.SetActive(false);
                ScoringOnDeath();
                
                // Particles System
                _laserParticles.Play();
                
                // Audio
                _audioSource.clip = AudioStock.Instance.LaserClips[0];
                _audioSource.Play();
            }
            
            // Display Laser RayCast
            Debug.DrawRay(transform.position, Vector3.down * _rayLaserLength, Color.red);
        }

        private void ScoringOnDeath()
        {
            if (_isplayerOneHit)
            {
                if (_ballGameObject)
                {
                    OldMatchManager.ScorePlayerTwo++;
                    OldMatchManager.PlayerOneScoreLast = true;
                    _oldMatchManager.DisplayScoreChange(false, true);
                    _cameraHandler.ScoredShake();
                    
                    Instantiate(_celebrationPointObject, new Vector3(8, 2, 0), Quaternion.identity);
                    
                    _ballGameObject.GetComponent<OldBallHandler>().Destroy();
                }
            }
            else
            {
                if (_ballGameObject)
                {
                    OldMatchManager.ScorePlayerOne++;
                    OldMatchManager.PlayerOneScoreLast = false;
                    _oldMatchManager.DisplayScoreChange(true, true);
                    _cameraHandler.ScoredShake();
                    
                    Instantiate(_celebrationPointObject, new Vector3(-8, 2, 0), Quaternion.identity);
                    
                    _ballGameObject.GetComponent<OldBallHandler>().Destroy();
                }
            }
        }
    }
}
