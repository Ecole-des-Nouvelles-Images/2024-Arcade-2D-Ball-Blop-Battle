using Int.Scripts.Utils;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class NewPlayerCountTouchBall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _maxTouchCount = 3;
        
        [Header("References")]
        [SerializeField] private NewBlopController _newBlopController;
        
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
            if (playerId != _newBlopController.PlayerId)
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
                    NewMatchManager.Instance.Foul(_newBlopController.PlayerId);
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
