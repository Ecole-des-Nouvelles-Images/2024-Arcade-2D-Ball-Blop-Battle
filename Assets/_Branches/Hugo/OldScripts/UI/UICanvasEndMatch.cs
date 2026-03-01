using _Branches.Hugo.OldScripts.Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UICanvasEndMatch : MonoBehaviour
    {
        [FormerlySerializedAs("_matchManager")]
        [Header("References")]
        [SerializeField] private OldMatchManager oldMatchManager;
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerOne;
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerTwo;
        [SerializeField] private TextMeshProUGUI _textWinLosePlayerOne;
        [SerializeField] private TextMeshProUGUI _textWinLosePlayerTwo;

        private void OnEnable()
        {
            _textCountSetScorePlayerOne.text = oldMatchManager.SetScorePlayerOne.ToString();
            _textCountSetScorePlayerTwo.text = oldMatchManager.SetScorePlayerTwo.ToString();

            if (oldMatchManager.SetScorePlayerOne > oldMatchManager.SetScorePlayerTwo)
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
