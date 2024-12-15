using Hugo.Prototype.Scripts.Game;
using TMPro;
using UnityEngine;

namespace Hugo.Prototype.Scripts.UI
{
    public class UICanvasEndMatch : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MatchManager _matchManager;
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerOne;
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerTwo;
        [SerializeField] private TextMeshProUGUI _textWinLosePlayerOne;
        [SerializeField] private TextMeshProUGUI _textWinLosePlayerTwo;

        private void OnEnable()
        {
            _textCountSetScorePlayerOne.text = _matchManager.SetScorePlayerOne.ToString();
            _textCountSetScorePlayerTwo.text = _matchManager.SetScorePlayerTwo.ToString();

            if (_matchManager.SetScorePlayerOne > _matchManager.SetScorePlayerTwo)
            {
                _textWinLosePlayerOne.color = Color.green;
                _textWinLosePlayerOne.text = "Gagné !";
                
                _textWinLosePlayerTwo.color = Color.red;
                _textWinLosePlayerTwo.text = "Perdu !";
            }
            else
            {
                _textWinLosePlayerOne.color = Color.red;
                _textWinLosePlayerOne.text = "Perdu !";
                
                _textWinLosePlayerTwo.color = Color.green;
                _textWinLosePlayerTwo.text = "Gagné !";
            }
        }
    }
}
