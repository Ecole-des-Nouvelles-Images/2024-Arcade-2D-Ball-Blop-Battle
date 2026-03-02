using Managers;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCountTouchBall : MonoBehaviour
    {
        public int CurrentTouchCount { get; private set; }
        
        [Header("Settings")]
        [SerializeField] private int _maxTouchCount = 3;
        [SerializeField] private float _hitInterval = 0.2f;
        
        [Header("References")]
        [SerializeField] private PlayerController playerController;

        private bool _canHit = true;
        private float _time;

        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnPlayerScored += OnPlayerScored;
            EventBus.OnPlayerTouchedBall += OnPlayerTouchedBall;
        }

        private void OnPlayerScored(int playerId)
        {
            ResetTouchCount();
        }
        
        private void OnPlayerTouchedBall(int playerId)
        {
            if (playerId != playerController.PlayerId)
            {
                ResetTouchCount();
            }
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= OnPlayerScored;
            EventBus.OnPlayerTouchedBall -= OnPlayerTouchedBall;
        }

        #endregion

        private void Update()
        {
            if (!_canHit)
            {
                _time += Time.deltaTime;

                if (_time >= _hitInterval)
                {
                    _time = 0f;
                    _canHit = true;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_canHit) return;
            
            if (other.gameObject.CompareTag("Ball"))
            {
                CurrentTouchCount++;
                Debug.Log("Touch : " + CurrentTouchCount);

                if (CurrentTouchCount >= _maxTouchCount)
                {
                    MatchManager.Instance.Foul(playerController.PlayerId);
                    ResetTouchCount();
                }

                _canHit = false;
            }
        }

        private void ResetTouchCount()
        {
            CurrentTouchCount = 0;
        }
    }
}