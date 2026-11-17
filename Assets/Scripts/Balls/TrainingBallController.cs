using Managers;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;

namespace Balls
{
    public class TrainingBallController : BaseBallController
    {
        [Header("===== TRAINING BALL SETTINGS =====")]
        [SerializeField] private bool _isCommitted = true;

        private float _gravityScale;

        private void Awake()
        {
            // Awake (et non Start) : la gravité doit être connue même si un blop touche la balle dès son apparition
            if (_isCommitted)
            {
                _gravityScale = _rb2d.gravityScale;
                _rb2d.gravityScale = 0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Avant le test d'absorption : si le PlayerController a traité le contact en premier et absorbé
            // la balle, l'engagement doit quand même avoir lieu (sinon balle tirée sans gravité)
            if (other.gameObject.CompareTag("Player")) Commit();

            if (_isAbsorbed) return;
            
        }

        public override void Absorb(Transform playerTransform)
        {
            Commit();
            base.Absorb(playerTransform);
        }

        /// <summary>Engagement : la balle flottante prend sa gravité au premier contact d'un blop.</summary>
        private void Commit()
        {
            if (!_isCommitted) return;

            _isCommitted = false;
            _rb2d.gravityScale = _gravityScale;

            EventBus.OnPlayerCommitment?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isAbsorbed) return;
            
            if (other.gameObject.CompareTag("PlayerOneSide"))
            {
                if (MatchManager.Instance != null)
                {
                    MatchManager.Instance.BallSide = 1;
                }
            }
            else if (other.gameObject.CompareTag("PlayerTwoSide"))
            {
                if (MatchManager.Instance != null)
                {
                    MatchManager.Instance.BallSide = 2;
                }
            }
        }

        private void IsDestroy()
        {
            if (MatchManager.Instance != null)
            {
                MatchManager.Instance.BallSide = 0;
            }
            
            Destroy(gameObject);
        }

        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
        }

        private void SpecialSpikeActivated(int playerId, BlopType blopType)
        {
            _rb2d.velocity /= 4;
            _rb2d.AddForce(Vector2.up * _speedSpecialSpikeActivation, ForceMode2D.Impulse);

            _isCommitted = false;
            _rb2d.gravityScale = _gravityScale;
            EventBus.OnPlayerCommitment?.Invoke();
        }
        
        private void OnDisable()
        {
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
        }

        #endregion
    }
}