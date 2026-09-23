using System;
using _Branches.Hugo.OldScripts.Ball;
using _Branches.Hugo.OldScripts.Player;
using _Branches.Hugo.OldScripts.UI;
using Cam;
using Celebrations;
using JetBrains.Annotations;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Singletons;
using Random = UnityEngine.Random;

namespace _Branches.Hugo.OldScripts.Game
{
    public class OldMatchManager : MonoBehaviourSingleton<OldMatchManager>
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
        [FormerlySerializedAs("_hudDisplay")] [SerializeField] private OldHUDDisplay oldHUDDisplay;
        [SerializeField] private CameraHandler _cameraHandler;
        
        [Header("Panels")]
        [SerializeField] private GameObject _playerOneFoulPanel;
        [SerializeField] private GameObject _playerTwoFoulPanel;
        [SerializeField] private GameObject _playerOneScoredPanel;
        [SerializeField] private GameObject _playerTwoScoredPanel;
        
        [Header("Canvas")]
        [SerializeField] private GameObject _canvasNewSet;
        [SerializeField] private GameObject _canvasEndMatch;
        
        [Header("Celebrations")]
        [SerializeField] private GameObject _celebrationPointObject;
        [SerializeField] private GameObject _celebrationSetObject;
        
        [Header("Public Variables")]
        public int SetScorePlayerOne;
        public int SetScorePlayerTwo;
        private bool _inGame;

        private void Update()
        {
            // if (_gameManager.FirstPlayerGameObject && _gameManager.SecondPlayerGameObject && !_inGame)
            // {
            //     _canvasNewSet.SetActive(true);
            //     Invoke(nameof(CanvasSetActiveFalse), _timeBetweenSets - 0.5f);
            //     
            //     Invoke(nameof(StartTimer), _timeBetweenSets);
            //     _inGame = true;
            // }
            
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
            // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().PerfectReceptionCount = 0;
            // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().CanSpecialSpike = false;
            // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerNumberTouchBallHandler>().NumberTouchBall = 0;
            // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().PerfectReceptionCount = 0;
            // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().CanSpecialSpike = false;
            // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerNumberTouchBallHandler>().NumberTouchBall = 0;

            GameObject celebrationSetGameObject;
            
            if (ScorePlayerOne > ScorePlayerTwo)
            {
                SetScorePlayerOne++;
                
                celebrationSetGameObject = Instantiate(_celebrationSetObject, new Vector3(0, -6, 0), Quaternion.identity);
                celebrationSetGameObject.GetComponent<CelebrationSetHandler>().SetUp(true, SetScorePlayerOne);
                
                if (SetScorePlayerOne == 3)
                {
                    // Debug.Log(" Player One WIN the match ");
                    
                    // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().WinTheMatch = true;
                    // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().LoseTheMatch = true;
                    //
                    // _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    // _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    
                    Instantiate(_celebrationPointObject, new Vector3(-8, 2, 0), Quaternion.identity);
                    
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
                
                celebrationSetGameObject = Instantiate(_celebrationSetObject, new Vector3(0, -6, 0), Quaternion.identity);
                celebrationSetGameObject.GetComponent<CelebrationSetHandler>().SetUp(false, SetScorePlayerTwo);
                
                if (SetScorePlayerTwo == 3)
                {
                    // Debug.Log(" Player Two WIN the match ");
                    
                    // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().LoseTheMatch = true;
                    // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().WinTheMatch = true;
                    //
                    // _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    // _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    
                    Instantiate(_celebrationPointObject, new Vector3(8, 2, 0), Quaternion.identity);
                    
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
                
                celebrationSetGameObject = Instantiate(_celebrationSetObject, new Vector3(0, -6, 0), Quaternion.identity);
                celebrationSetGameObject.GetComponent<CelebrationSetHandler>().SetUp(true, SetScorePlayerOne);
                
                if (SetScorePlayerOne == 3)
                {
                    // Debug.Log(" Player One WIN the match ");
                    
                    // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().WinTheMatch = true;
                    // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().LoseTheMatch = true;
                    //
                    // _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    // _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerTwo == 3)
                {
                    // Debug.Log(" Player Two WIN the match ");
                    
                    // _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().LoseTheMatch = true;
                    // _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().WinTheMatch = true;
                    //
                    // _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    // _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    Invoke(nameof(EndGame), 5f);
                }
                if (SetScorePlayerOne == 3 && SetScorePlayerTwo == 3)
                {
                    // _gameManager.FirstPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
                    // _gameManager.SecondPlayerGameObject.GetComponent<PlayerInputHandler>().InputAreEnable = false;
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
                _ballPrefab.GetComponent<OldBallHandler>().DirectionCommitment = new Vector2(-4, -1);
                
                // float randomNumberX = Random.Range(0.5f, 1.1f);
                // _ballPrefab.GetComponent<BallHandler>().DirectionCommitment = new Vector2(randomNumberX, randomNumberY);
            }
            else
            {
                _ballPrefab.GetComponent<OldBallHandler>().DirectionCommitment = new Vector2(4, -1);
                
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
            oldHUDDisplay.DisplayScoreChange(isPlayerOneScored);
            
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
