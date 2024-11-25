using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class MatchManager : MonoBehaviourSingleton<MatchManager>
    {
        [Header("Scoring")]
        public static int ScorePlayerOne;
        public static int ScorePlayerTwo;
        public static bool PlayerOneScoreLast = true;

        [Header("Timer")]
        [SerializeField] private float _totalTimer;
        public static float CurrentTime;
        private bool _isTimerRunning;

        [Header("Commitment")]
        public static bool IsBallInGame = true;

        [Header("Prefabs")]
        [SerializeField] private GameObject _ball;

        private void Start()
        {
            StartTimer();
        }

        private void Update()
        {
            // Timer
            if (_isTimerRunning)
            {
                CurrentTime -= Time.deltaTime;

                if (CurrentTime <= 0f)
                {
                    _isTimerRunning = false;
                    CurrentTime = 0f;
                    OnTimerEnd();
                }
            }
            
            // Commitment
            if (IsBallInGame == false)
            {
                Commitment();
            }
        }
        
        private void StartTimer()
        {
            CurrentTime = _totalTimer;
            _isTimerRunning = true;
        }

        private void OnTimerEnd()
        {
            Debug.Log("Timer End");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Commitment()
        {
            if (PlayerOneScoreLast)
            {
                _ball.GetComponent<BallHandler>().DirectionCommitment = new Vector2(1,1);
            }
            else
            {
                _ball.GetComponent<BallHandler>().DirectionCommitment = new Vector2(-1,1);
            }
            
            Instantiate(_ball, transform.position, Quaternion.identity);
            
            IsBallInGame = true;
        }
    }
}
