using System.Collections.Generic;
using Int.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Refacto.Scripts
{
    public class DisplayMatchInfo : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private TextMeshProUGUI _scorePlayerOne;
        [SerializeField] private TextMeshProUGUI _scorePlayerTwo;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private List<Image> _setsPlayerOne;
        [SerializeField] private List<Image> _setsPlayerTwo;

        private void Update()
        {
            _scorePlayerOne.text = NewMatchManager.Instance.PlayerOneScore.ToString();
            _scorePlayerTwo.text = NewMatchManager.Instance.PlayerTwoScore.ToString();

            float time = NewMatchManager.Instance.TimerHandler.CurrentTimer;
            if (time < 10f)
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
                for (int i = 0; i < NewMatchManager.Instance.PlayerOneSetCount; i++)
                {
                    _setsPlayerOne[i].color = Color.green;
                }
            }
            else if (playerId == 2)
            {
                for (int i = 0; i < NewMatchManager.Instance.PlayerTwoSetCount; i++)
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