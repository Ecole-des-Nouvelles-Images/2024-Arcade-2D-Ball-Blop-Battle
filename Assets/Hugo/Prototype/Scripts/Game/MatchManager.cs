using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Player;
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
        public static bool IsSetOver;
        
        [Header("References")]
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _panelPaused;
        [SerializeField] private GameManager _gameManager;
        
        private int _setScorePlayerOne;
        private int _setScorePlayerTwo;

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
            //Start Timer
            CurrentTime = _totalTimer;
            _isTimerRunning = true;
            
            // Reset Values
            IsSetOver = false;
            ScorePlayerOne = 0;
            ScorePlayerTwo = 0;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnTimerEnd()
        {
            IsSetOver = true;
            _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
            _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().CanSpecialSpike = false;
            _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
            _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().CanSpecialSpike = false;
            
            if (ScorePlayerOne > ScorePlayerTwo)
            {
                _setScorePlayerOne++;
                
                if (_setScorePlayerOne == 2)
                {
                    Debug.Log(" Player One WIN the match ");
                    
                    EndGame();
                }
                else
                {
                    Invoke(nameof(Commitment), 2f);
                    Invoke(nameof(StartTimer), 2f);
                }
            }
            else if (ScorePlayerOne < ScorePlayerTwo)
            {
                _setScorePlayerTwo++;
                
                if (_setScorePlayerTwo == 2)
                {
                    Debug.Log(" Player Two WIN the match ");
                    
                    EndGame();
                }
                else
                {
                    Invoke(nameof(Commitment), 2f);
                    Invoke(nameof(StartTimer), 2f);
                }
            }
            else
            {
                _setScorePlayerOne++;
                _setScorePlayerTwo++;
                
                Invoke(nameof(Commitment), 2f);
                Invoke(nameof(StartTimer), 2f);
            }
            
            Debug.Log(" Player One : " + _setScorePlayerOne + " / " + _setScorePlayerTwo + " : Player Two ");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Commitment()
        {
            if (PlayerOneScoreLast)
            {
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(1,1);
            }
            else
            {
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(-1,1);
            }
            
            Instantiate(_ballPrefab, transform.position, Quaternion.identity);
            
            IsBallInGame = true;
        }

        private void EndGame()
        {
            _panelPaused.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
