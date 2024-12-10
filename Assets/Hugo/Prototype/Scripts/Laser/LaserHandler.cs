using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Laser
{
    public class LaserHandler : MonoBehaviour
    {
        [Header("Laser Settings")]
        [SerializeField] private float _rayLaserLength;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _ballLayer;
        [SerializeField] private ParticleSystem _laserParticles;

        private bool _hasAlreadyHit;
        private bool _isplayerOneHit;
        private GameObject _ballGameObject;
        private MatchManager _matchManager;
        
        private void Awake()
        {
            _matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        }

        private void Update()
        {
            // Raycast _laserHit
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, Vector3.down, _rayLaserLength, _playerLayer);
            RaycastHit2D hitBall = Physics2D.Raycast(transform.position, Vector3.down, _rayLaserLength, _ballLayer);
            if (hitBall.collider)
            {
                _ballGameObject = hitBall.collider.gameObject;
                //Debug.Log(_ballGameObject);
            }
            
            if (hitPlayer.collider && !_hasAlreadyHit)
            {
                _hasAlreadyHit = true;
                
                //Debug.Log(" hit player ");
                GameObject playerChildren = hitPlayer.collider.gameObject;
                playerChildren.GetComponentInParent<PlayerController>().PlayerDie();
                _isplayerOneHit = playerChildren.GetComponentInParent<PlayerNumberTouchBallHandler>().IsPlayerOne;
                
                // Particles System
                _laserParticles.Play();
                
                ScoringOnDeath();
                Invoke(nameof(ResetHasAlreadyHit), 0.5f);
            }
            
            // Display Laser RayCast
            Debug.DrawRay(transform.position, Vector3.down * _rayLaserLength, Color.red);
        }

        private void ScoringOnDeath()
        {
            if (_isplayerOneHit)
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
                _matchManager.DisplayScoreChange(false);
                Destroy(_ballGameObject);
            }
            else
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                _matchManager.DisplayScoreChange(true);
                Destroy(_ballGameObject);
            }
        }

        private void ResetHasAlreadyHit()
        {
            _hasAlreadyHit = false;
        }
    }
}
