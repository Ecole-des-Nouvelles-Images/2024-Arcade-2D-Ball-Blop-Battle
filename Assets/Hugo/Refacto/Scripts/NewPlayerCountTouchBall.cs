using System;
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
            EventBus.OnPlayerScored += ResetTouchCount;
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= ResetTouchCount;
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
                    ResetTouchCount(0);
                }
            }
        }

        private void ResetTouchCount(int playerId)
        {
            _currentTouchCount = 0;
        }
    }
}
