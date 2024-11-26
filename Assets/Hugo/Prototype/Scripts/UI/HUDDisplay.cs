using Hugo.Prototype.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class HUDDisplay : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private Text _textTimer;
        [SerializeField] private Text _textScorePlayerOne;
        [SerializeField] private Text _textScorePlayerTwo;

        private void Update()
        {
            _textScorePlayerOne.text = MatchManager.ScorePlayerOne.ToString();
            _textScorePlayerTwo.text = MatchManager.ScorePlayerTwo.ToString();
            _textTimer.text = Mathf.RoundToInt(MatchManager.CurrentTime).ToString();
        }
    }
}