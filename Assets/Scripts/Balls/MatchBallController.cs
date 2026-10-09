using Managers;
using Player.ScriptableObjects;
using UnityEngine;
using EventBus = Utils.EventBus;

namespace Balls
{
    public class MatchBallController : BaseBallController
    {
        [Header("===== MATCH BALL SETTINGS =====")]
        [SerializeField] private bool _isCommitted = true;

        private float _gravityScale;

        private void Start()
        {
            if (_isCommitted)
            {
                _gravityScale = _rb2d.gravityScale;
                _rb2d.gravityScale = 0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isAbsorbed) return;
            
            if (other.gameObject.CompareTag("PlayerOneGround"))
            {
                EventBus.OnPlayerScored?.Invoke(2);
                IsDestroy();
            }
            else if (other.gameObject.CompareTag("PlayerTwoGround"))
            {
                EventBus.OnPlayerScored?.Invoke(1);
                IsDestroy();
            }
            
            if (other.gameObject.CompareTag("Player") && _isCommitted)
            {
                _isCommitted = false;
                _rb2d.gravityScale = _gravityScale;
                
                EventBus.OnPlayerCommitment?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isAbsorbed) return;

            if (other.gameObject.CompareTag("PlayerOneGround"))
            {
                if (MatchManager.Instance != null)
                {
                    MatchManager.Instance.Foul(1);
                }
                IsDestroy();
            }
            else if (other.gameObject.CompareTag("PlayerTwoGround"))
            {
                if (MatchManager.Instance != null)
                {
                    MatchManager.Instance.Foul(2);
                }
                IsDestroy();
            }
            
            if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Selling"))
            {
                Vector2 direction = _rb2d.velocity;
                Vector2 newDirection = direction;
                newDirection.x = -_rb2d.velocity.x;
                _rb2d.velocity = newDirection;
            }
            
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

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _col2D.isTrigger = false;
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