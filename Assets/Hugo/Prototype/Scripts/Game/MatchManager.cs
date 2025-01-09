using System;
using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Camera;
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
        public bool IsTimerRunning;

        [Header("Commitment")]
        public static bool IsSetOver;
        [SerializeField] private float _timerNewBall;
        
        [Header("References")]
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private HUDDisplay _hudDisplay;
        [SerializeField] private CameraHandler _cameraHandler;
        
        [Header("Panels")]
        [SerializeField] private GameObject _playerOneFoulPanel;
        [SerializeField] private GameObject _playerTwoFoulPanel;
        [SerializeField] private GameObject _playerOneScoredPanel;
        [SerializeField] private GameObject _playerTwoScoredPanel;
        
        [Header("Canvas")]
        [SerializeField] private GameObject _canvasNewSet;
        [SerializeField] private GameObject _canvasEndMatch;
        
        [Header("Public Variables")]
        public int SetScorePlayerOne;
        public int SetScorePlayerTwo;
        private bool _inGame;

        private void Update()
        {
            if (_gameManager.FirstPlayerGameObject && _gameManager.SecondPlayerGameObject && !_inGame)
            {
                _canvasNewSet.SetActive(true);
                Invoke(nameof(CanvasSetActiveFalse), _timeBetweenSets - 0.5f);
                
                Invoke(nameof(StartTimer), _timeBetweenSets);
                _inGame = true;
            }
            
            // Timer
            if (IsTimerRunning)
            {
                CurrentTime -= Time.deltaTime;

                if (CurrentTime <= 0f)
                {
                    IsTimerRunning = false;
                    CurrentTime = 0f;
                    OnTimerEnd();
                }
            }
        }
        
        private void StartTimer()
        {
            //Start Timer
            CurrentTime = _totalTimer;
            // IsTimerRunning = true;
            
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
                    // Debug.Log(" Player One WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    _canvasNewSet.SetActive(true);
                    Invoke(nameof(CanvasSetActiveFalse), _timeBetweenSets - 0.5f);
                    
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            else if (ScorePlayerOne < ScorePlayerTwo)
            {
                SetScorePlayerTwo++;
                
                if (SetScorePlayerTwo == 3)
                {
                    // Debug.Log(" Player Two WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    _canvasNewSet.SetActive(true);
                    Invoke(nameof(CanvasSetActiveFalse), _timeBetweenSets - 0.5f);
                    
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            else
            {
                SetScorePlayerOne++;
                SetScorePlayerTwo++;
                
                if (SetScorePlayerOne == 3)
                {
                    // Debug.Log(" Player One WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerTwo == 3)
                {
                    // Debug.Log(" Player Two WIN the match ");
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().LoseTheMatch = true;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().WinTheMatch = true;
                    
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerOne == 3 && SetScorePlayerTwo == 3)
                {
                    _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                else
                {
                    _canvasNewSet.SetActive(true);
                    Invoke(nameof(CanvasSetActiveFalse), _timeBetweenSets - 0.5f);
                    
                    Invoke(nameof(StartTimer), _timeBetweenSets);
                }
            }
            
            // Debug.Log(" Player One : " + SetScorePlayerOne + " / " + SetScorePlayerTwo + " : Player Two ");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Commitment()
        {
            float randomNumberY = Random.Range(0.1f, 1.1f);
            if (PlayerOneScoreLast)
            {
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(-4, -1);
                
                // float randomNumberX = Random.Range(0.5f, 1.1f);
                // _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(randomNumberX, randomNumberY);
            }
            else
            {
                _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(4, -1);
                
                // float randomNumberX = Random.Range(-1.1f, -0.5f);
                // _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(randomNumberX, randomNumberY);
            }
            
            Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        }

        private void EndGame()
        {
            _canvasEndMatch.SetActive(true);
        }

        public void DisplayScoreChange(bool isPlayerOneScored, bool isFoul)
        {
            _hudDisplay.DisplayScoreChange(isPlayerOneScored);
            
            if (isPlayerOneScored)
            {
                if (_playerOneScoredPanel)
                {
                    _playerOneScoredPanel.SetActive(true);
                }
            }
            else
            {
                if (_playerTwoScoredPanel)
                {
                    _playerTwoScoredPanel.SetActive(true);
                }
            }
            
            if (isFoul)
            {
                DisplayFouls(isPlayerOneScored);
            }
        }

        private void DisplayFouls(bool isPlayerOneScored)
        {
            if (isPlayerOneScored)
            {
                if (_playerTwoFoulPanel)
                {
                    _playerTwoFoulPanel.SetActive(true);
                }
            }
            else
            {
                if (_playerOneFoulPanel)
                {
                    _playerOneFoulPanel.SetActive(true);
                }
            }
        }

        private void CanvasSetActiveFalse()
        {
            _canvasNewSet.SetActive(false);
        }
        
        // Utils
        public void InvokeMethodTimer([NotNull] string methodName)
        {
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            Invoke(methodName, _timerNewBall);
        }
    }
}
