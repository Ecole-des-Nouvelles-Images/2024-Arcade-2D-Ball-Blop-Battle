using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using Utils.Singletons;

namespace Managers
{
    [RequireComponent(typeof(TimerHandler))]
    public class MatchManager : MonoBehaviourSingleton<MatchManager>
    {
        // POINTS
        public int PlayerOneScore { get; private set; }
        public int PlayerOneSetCount { get; private set; }
        public int PlayerTwoScore { get; private set; }
        public int PlayerTwoSetCount { get; private set; }
        
        // TIMER
        public TimerHandler TimerHandler { get; private set; }
        
        [Header("Match Infos")]
        public int BallSide;
        
        [Header("Match Settings")]
        [SerializeField] private float _setDuration;
        [SerializeField] private int _setCountToWinAMatch;
        [SerializeField] private float _timeBetweenCommitments;
        [SerializeField] private float _timeBetweenSets;
        
        [Header("Commitment Settings")]
        [SerializeField] private Vector2 _playerOneCommitmentPos;
        [SerializeField] private Vector2 _playerTwoCommitmentPos;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _ball;
        
        private GameObject _currentBall;

        private bool _isPlaying;
        private bool _isSetOver;
        private Coroutine _commitmentCoroutine;

        private void Awake()
        {
            TimerHandler = GetComponent<TimerHandler>();
            TimerHandler.Setup(_setDuration);
        }

        private void Start()
        {
            if (_commitmentCoroutine != null) return;
            _commitmentCoroutine = StartCoroutine(CommitmentCoroutine(true, Random.Range(1, 3), _timeBetweenSets));
        }

        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnPlayerScored += PlayerScored;
            EventBus.OnPlayerCommitment += PlayerCommitment;
            EventBus.OnSetIsOver += SetIsOver;
        }

        private void PlayerScored(int scoringPlayerId)
        {
            TimerHandler.StopTimer();
            
            if (scoringPlayerId == 1)
            {
                PlayerOneScore++;
            }
            else if (scoringPlayerId == 2)
            {
                PlayerTwoScore++;
            }
            
            Debug.Log("Score  : " + PlayerOneScore + " / " + PlayerTwoScore);

            if (!_isSetOver)
            {
                if (_commitmentCoroutine != null) return;
                _commitmentCoroutine = StartCoroutine(CommitmentCoroutine(false, scoringPlayerId, _timeBetweenCommitments));
            }
            else if (_isSetOver && PlayerOneScore != PlayerTwoScore)
            {
                if (PlayerOneScore > PlayerTwoScore)
                {
                    PlayerOneSetCount++;
                    SetOver(1);

                    EventBus.OnSetChangement?.Invoke(1);
                }
                else
                {
                    PlayerTwoSetCount++;
                    SetOver(2);

                    EventBus.OnSetChangement?.Invoke(2);
                }
                
                Debug.Log("Set  : " + PlayerOneSetCount + " / " + PlayerTwoSetCount);
            }
            else if (_isSetOver && PlayerOneScore == PlayerTwoScore)
            {
                if (_commitmentCoroutine != null) return;
                _commitmentCoroutine = StartCoroutine(CommitmentCoroutine(false, scoringPlayerId, _timeBetweenCommitments));
            }
        }
        
        private void PlayerCommitment()
        {
            if (PlayerOneScore == 0 && PlayerTwoScore == 0)
            {
                TimerHandler.StartTimer();
            }
            else
            {
                TimerHandler.ResumeTimer();
            }
        }
        
        private void SetIsOver()
        {
            _isSetOver = true;
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= PlayerScored;
            EventBus.OnPlayerCommitment -= PlayerCommitment;
            EventBus.OnSetIsOver -= SetIsOver;

        }

        #endregion

        private void SetOver(int winSetPlayerId)
        {
            if (PlayerOneSetCount < _setCountToWinAMatch && PlayerTwoSetCount < _setCountToWinAMatch)
            {
                _isSetOver = false;
                if (_commitmentCoroutine != null) return;
                _commitmentCoroutine = StartCoroutine(CommitmentCoroutine(true, winSetPlayerId, _timeBetweenSets));
            }
            else if (PlayerOneSetCount == _setCountToWinAMatch)
            {
                Debug.Log("VICTORY PLAYER ONE");
                SceneManager.LoadScene(1);
            }
            else if (PlayerTwoSetCount == _setCountToWinAMatch)
            {
                Debug.Log("VICTORY PLAYER TWO");
                SceneManager.LoadScene(1);
            }
        }
        
        private IEnumerator CommitmentCoroutine(bool firstCommitment, int scoringPlayerId, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            if (_currentBall) Destroy(_currentBall);
            
            if (scoringPlayerId == 1)
            {
                _currentBall = Instantiate(_ball, _playerTwoCommitmentPos, _ball.transform.rotation);
            }
            else if (scoringPlayerId == 2)
            {
                _currentBall = Instantiate(_ball, _playerOneCommitmentPos, _ball.transform.rotation);
            }

            if (firstCommitment)
            {
                PlayerOneScore = 0;
                PlayerTwoScore = 0;
                TimerHandler.ResetTimer();
            }

            _isPlaying = true;
            _commitmentCoroutine = null;
        }

        public void Foul(int playerId)
        {
            if (!_isPlaying || _commitmentCoroutine != null) return;
            
            Debug.Log("FOUL");
            
            if (_currentBall) Destroy(_currentBall);
            EventBus.OnFoul?.Invoke();
            
            _isPlaying = false;

            if (playerId == 1)
            {
                PlayerScored(2);
            }
            else if (playerId == 2)
            {
                PlayerScored(1);
            }
        }
    }
}
