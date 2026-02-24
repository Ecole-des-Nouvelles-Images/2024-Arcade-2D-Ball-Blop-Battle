using System.Collections;
using Int.Scripts.Utils;
using Int.Scripts.Utils.Singletons;
using UnityEngine;

namespace Hugo.Refacto.Scripts
{
    [RequireComponent(typeof(TimerHandler))]
    public class NewMatchManager : MonoBehaviourSingleton<NewMatchManager>
    {
        // POINTS
        public int PlayerOneScore { get; private set; }
        public int PlayerOneSetCount { get; private set; }
        public int PlayerTwoScore { get; private set; }
        public int PlayerTwoSetCount { get; private set; }
        
        // Timer
        public TimerHandler TimerHandler { get; private set; }
        
        [Header("Match Settings")]
        [SerializeField] private float _setDuration;
        [SerializeField] private int _setCountToWinAMatch;
        [SerializeField] private float _timeBetweenCommitments;
        [SerializeField] private float _timeBetweenSets;
        
        [Header("Commitment Settings")]
        [SerializeField] private Vector2 _playerOneCommitmentPos;
        [SerializeField] private Vector2 _playerTwoCommitmentPos;
        
        [Header("References")]
        [SerializeField] private GameObject _ball;

        private bool _isSetOver;

        private void Awake()
        {
            TimerHandler = GetComponent<TimerHandler>();
            TimerHandler.Setup(_setDuration);
        }

        #region Events

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
                StartCoroutine(CommitmentCoroutine(scoringPlayerId, _timeBetweenCommitments));
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
                StartCoroutine(CommitmentCoroutine(scoringPlayerId, _timeBetweenCommitments));
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
                PlayerOneScore = 0;
                PlayerTwoScore = 0;
                StartCoroutine(CommitmentCoroutine(winSetPlayerId, _timeBetweenSets));
            }
            else if (PlayerOneSetCount == _setCountToWinAMatch)
            {
                Debug.Log("VICTORY PLAYER ONE");
            }
            else if (PlayerTwoSetCount == _setCountToWinAMatch)
            {
                Debug.Log("VICTORY PLAYER TWO");
            }
        }
        
        private IEnumerator CommitmentCoroutine(int scoringPlayerId, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            if (scoringPlayerId == 1)
            {
                Instantiate(_ball, _playerTwoCommitmentPos, _ball.transform.rotation);
            }
            else if (scoringPlayerId == 2)
            {
                Instantiate(_ball, _playerOneCommitmentPos, _ball.transform.rotation);
            }
        }
    }
}
