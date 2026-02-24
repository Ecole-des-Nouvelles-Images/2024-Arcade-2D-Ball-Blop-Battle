using Int.Scripts.Utils;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    public class TimerHandler : MonoBehaviour
    {
        public float CurrentTimer { get; private set; }

        private float _setDuration;
        private bool _isRunning;

        public void Setup(float setDuration)
        {
            _setDuration = setDuration;
            CurrentTimer = _setDuration;
        }

        public void StartTimer()
        {
            CurrentTimer = _setDuration;
            _isRunning = true;
        }
        
        public void ResumeTimer()
        {
            _isRunning = true;
        }
        
        public void StopTimer()
        {
            _isRunning = false;
        }

        private void Update()
        {
            if (!_isRunning) return;
            
            CurrentTimer -= Time.deltaTime;

            if (CurrentTimer <= 0f)
            {
                _isRunning = false;
                CurrentTimer = 0f;
                
                EventBus.OnSetIsOver?.Invoke();
            }
        }
    }
}
