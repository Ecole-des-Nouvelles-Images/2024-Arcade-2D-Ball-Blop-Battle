using System;
using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Player;
using Hugo.Prototype.Scripts.UI;
using Hugo.Prototype.Scripts.Utils;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

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
        [SerializeField] private float _timeBetweenSets;
        public static float CurrentTime;
        private bool _isTimerRunning;

        [Header("Commitment")]
        public static bool IsSetOver;
        [SerializeField] private float _timerNewBall;
        
        [Header("References")]
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _panelPaused;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private HUDDisplay _hudDisplay;
        
        [Header("Panels")]
        [SerializeField] private GameObject _playerOneFoulPanel;
        [SerializeField] private GameObject _playerTwoFoulPanel;
        
        [Header("Public Variables")]
        public int SetScorePlayerOne;
        public int SetScorePlayerTwo;
        private bool _inGame;

        private void Update()
        {
            if (_gameManager.FirstPlayerGameObject && _gameManager.SecondPlayerGameObject && !_inGame)
            {
                Invoke(nameof(StartTimer), 3f);
                _inGame = true;
            }
            
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
            
            Commitment();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void OnTimerEnd()
        {
            IsSetOver = true;
            _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
            _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().CanSpecialSpike = false;
            _gameManager.FirstPlayerGameObject.GetComponent<PlayerNumberTouchBallHandler>().NumberTouchBall = 0;
            _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount = 0;
            _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().CanSpecialSpike = false;
            _gameManager.SecondPlayerGameObject.GetComponent<PlayerNumberTouchBallHandler>().NumberTouchBall = 0;
            
            if (ScorePlayerOne > ScorePlayerTwo)
            {
                SetScorePlayerOne++;
                
                if (SetScorePlayerOne == 3)
                {
                    Debug.Log(" Player One WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            else if (ScorePlayerOne < ScorePlayerTwo)
            {
                SetScorePlayerTwo++;
                
                if (SetScorePlayerTwo == 3)
                {
                    Debug.Log(" Player Two WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            else
            {
                SetScorePlayerOne++;
                SetScorePlayerTwo++;
                
                if (SetScorePlayerOne == 3)
                {
                    Debug.Log(" Player One WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerTwo == 3)
                {
                    Debug.Log(" Player Two WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerOne == 3 && SetScorePlayerTwo == 3)
                {
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            
            Debug.Log(" Player One : " + SetScorePlayerOne + " / " + SetScorePlayerTwo + " : Player Two ");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Commitment()
        {
            float randomNumberY = Random.Range(0.1f, 1.1f);
            if (PlayerOneScoreLast)
            {
                float randomNumberX = Random.Range(0.5f, 1.1f);
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(randomNumberX, randomNumberY);
            }
            else
            {
                float randomNumberX = Random.Range(-1.1f, -0.5f);
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(randomNumberX, randomNumberY);
            }
            
            Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        }

        private void EndGame()
        {
            _panelPaused.SetActive(true);
            Time.timeScale = 0f;
        }

        public void DisplayScoreChange(bool isPlayerOneScored, bool isFoul)
        {
            _hudDisplay.DisplayScoreChange(isPlayerOneScored);
            if (isFoul)
            {
                DisplayFouls(isPlayerOneScored);
            }
        }

        private void DisplayFouls(bool isPlayerOneScored)
        {
            if (isPlayerOneScored)
            {
                _playerTwoFoulPanel.SetActive(true);
            }
            else
            {
                _playerOneFoulPanel.SetActive(true);
            }
        }
        
        // Utils
        public void InvokeMethodTimer([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Invoke(methodName, _timerNewBall);
        }
    }
}
