using Managers;
using TMPro;
using UnityEngine;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UICanvasEndMatch : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerOne;
        [SerializeField] private TextMeshProUGUI _textCountSetScorePlayerTwo;
        [SerializeField] private TextMeshProUGUI _textWin;

        private void OnEnable()
        {
            _textCountSetScorePlayerOne.text = MatchManager.Instance.PlayerOneSetCount.ToString();
            _textCountSetScorePlayerTwo.text = MatchManager.Instance.PlayerTwoSetCount.ToString();

            if (MatchManager.Instance.PlayerOneSetCount > MatchManager.Instance.PlayerTwoSetCount)
            {
                _textWin.text = " Victoire joueur 1 !";
                _textCountSetScorePlayerOne.color = Color.green;
                _textCountSetScorePlayerTwo.color = Color.red;
            }
            else
            {
                _textWin.text = " Victoire joueur 2 !";
                _textCountSetScorePlayerOne.color = Color.red;
                _textCountSetScorePlayerTwo.color = Color.green;
            }
        }
    }
}