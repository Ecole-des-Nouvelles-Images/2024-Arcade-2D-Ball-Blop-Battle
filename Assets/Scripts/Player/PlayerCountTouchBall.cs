using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Player
{
    public class PlayerCountTouchBall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _maxTouchCount = 3;
        
        [FormerlySerializedAs("blopController")]
        [FormerlySerializedAs("_newBlopController")]
        [Header("References")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Debug")]
        [SerializeField] private int _currentTouchCount;

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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _currentTouchCount++;
                Debug.Log("Touch : " + _currentTouchCount);

                if (_currentTouchCount >= _maxTouchCount)
                {
                    MatchManager.Instance.Foul(playerController.PlayerId);
                    ResetTouchCount();
                }
            }
        }

        private void ResetTouchCount()
        {
            _currentTouchCount = 0;
        }
    }
}
