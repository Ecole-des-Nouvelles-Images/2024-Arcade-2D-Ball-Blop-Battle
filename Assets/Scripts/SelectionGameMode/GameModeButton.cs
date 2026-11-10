using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionGameMode
{
    public class GameModeButton : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Image _firstOponent;
        [SerializeField] private Image _secondOponent;
        [SerializeField] private TextMeshProUGUI _tmpDescription;
        
        private GameModeData _gameModeData;
        
        public void Setup(GameModeData gameModeData)
        {
            _gameModeData = gameModeData;

            _firstOponent.sprite = _gameModeData.FirstOponent;
            _secondOponent.sprite = _gameModeData.SecondOponent;
            _tmpDescription.text = _gameModeData.Text;
        }
    }
}