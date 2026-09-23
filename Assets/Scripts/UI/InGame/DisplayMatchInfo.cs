using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.InGame
{
    public class DisplayMatchInfo : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private TextMeshProUGUI _scorePlayerOne;
        [SerializeField] private TextMeshProUGUI _scorePlayerTwo;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private Transform _containerSetsPlayerOne;
        [SerializeField] private Transform _containerSetsPlayerTwo;
        
        [Header("===== PREFABS =====")]
        [SerializeField] private GameObject _prefabImageSet;
        
        private List<Image> _setsPlayerOne = new();
        private List<Image> _setsPlayerTwo = new();

        private void Start()
        {
            for (int i = 0; i < GameManager.Instance.SetCountToWinAMatch; i++)
            {
                GameObject goP1 = Instantiate(_prefabImageSet, _containerSetsPlayerOne);
                _setsPlayerOne.Add(goP1.GetComponent<Image>());
                
                GameObject goP2 =Instantiate(_prefabImageSet, _containerSetsPlayerTwo);
                _setsPlayerTwo.Add(goP2.GetComponent<Image>());
            }
        }

        private void Update()
        {
            _scorePlayerOne.text = MatchManager.Instance.PlayerOneScore.ToString();
            _scorePlayerTwo.text = MatchManager.Instance.PlayerTwoScore.ToString();

            float time = MatchManager.Instance.TimerHandler.CurrentTimer;
            if (time <= 9.9f)
            {
                _timer.text = time.ToString("F1");
            }
            else
            {
                _timer.text = Mathf.FloorToInt(time).ToString();
            }
        }
        
        #region Events

        private void OnEnable()
        {
            EventBus.OnPlayerScored += PlayerScored;
            EventBus.OnSetChangement += SetChangement;
        }

        private void PlayerScored(int scoringPlayerId)
        {
            // ANIMATION Grossissement
            if (scoringPlayerId == 1)
            {
                
            }
            else if (scoringPlayerId == 2)
            {
                
            }
        }
        
        private void SetChangement(int playerId)
        {
            if (playerId == 1)
            {
                for (int i = 0; i < MatchManager.Instance.PlayerOneSetCount; i++)
                {
                    _setsPlayerOne[i].color = Color.green;
                }
            }
            else if (playerId == 2)
            {
                for (int i = 0; i < MatchManager.Instance.PlayerTwoSetCount; i++)
                {
                    _setsPlayerTwo[i].color = Color.green;
                }
            }
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerScored -= PlayerScored;
            EventBus.OnSetChangement -= SetChangement;
        }

        #endregion
    }
}