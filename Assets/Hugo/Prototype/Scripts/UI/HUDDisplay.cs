using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class HUDDisplay : MonoBehaviour
    {
        [Header("Game Manager")]
        [SerializeField] private GameManager _gameManager;
        
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI _textTimer;
        [SerializeField] private TextMeshProUGUI _textScorePlayerOne;
        [SerializeField] private TextMeshProUGUI _textScorePlayerTwo;
        [SerializeField] private Image _firstPerfectReceptionPlayerOne;
        [SerializeField] private Image _secondPerfectReceptionPlayerOne;
        [SerializeField] private Image _thirdPerfectReceptionPlayerOne;
        [SerializeField] private Image _firstPerfectReceptionPlayerTwo;
        [SerializeField] private Image _secondPerfectReceptionPlayerTwo;
        [SerializeField] private Image _thirdPerfectReceptionPlayerTwo;
        
        private void Update()
        {
            // Timer and Scoring
            _textScorePlayerOne.text = MatchManager.ScorePlayerOne.ToString();
            _textScorePlayerTwo.text = MatchManager.ScorePlayerTwo.ToString();
            _textTimer.text = Mathf.RoundToInt(MatchManager.CurrentTime).ToString();
            
            // Perfect Reception
            // Player One
            if (_gameManager.FirstPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount;
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.white;
                    _secondPerfectReceptionPlayerOne.color = Color.white;
                    _thirdPerfectReceptionPlayerOne.color = Color.white;
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.yellow;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerOne.color = Color.yellow;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.red;
                    _secondPerfectReceptionPlayerOne.color = Color.red;
                    _thirdPerfectReceptionPlayerOne.color = Color.red;
                }
            }
            else
            {
                _firstPerfectReceptionPlayerOne.color = Color.white;
                _secondPerfectReceptionPlayerOne.color = Color.white;
                _thirdPerfectReceptionPlayerOne.color = Color.white;
            }
            
            // Player Two
            if (_gameManager.SecondPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount;
                
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.white;
                    _secondPerfectReceptionPlayerTwo.color = Color.white;
                    _thirdPerfectReceptionPlayerTwo.color = Color.white;
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.yellow;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerTwo.color = Color.yellow;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.red;
                    _secondPerfectReceptionPlayerTwo.color = Color.red;
                    _thirdPerfectReceptionPlayerTwo.color = Color.red;
                }
            }
            else
            {
                _firstPerfectReceptionPlayerTwo.color = Color.white;
                _secondPerfectReceptionPlayerTwo.color = Color.white;
                _thirdPerfectReceptionPlayerTwo.color = Color.white;
            }
        }
    }
}