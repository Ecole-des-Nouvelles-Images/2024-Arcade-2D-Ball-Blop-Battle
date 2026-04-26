using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace SelectionGameMode
{
    public class GameModeSelectionHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Slider _setCountSlider;

        public void GameModeSelected(string gameMode)
        {
            GameManager.Instance.GameMode = gameMode;
            
            if (gameMode == "Training")
            {
                Debug.Log(" SOON ");
            }
            else if (gameMode == "Duel")
            {
                SceneLoaderManager.Instance.LoadScene("BlopSelection");
            }
        }
        
        private void Start()
        {
            _timerSlider.onValueChanged.AddListener(OnTimerChanged);
            _setCountSlider.onValueChanged.AddListener(OnSetCountChanged);

            _timerSlider.value = GameManager.Instance.SetDuration;
            _setCountSlider.value = GameManager.Instance.SetCountToWinAMatch;
        }

        private void OnTimerChanged(float value)
        {
            GameManager.Instance.SetDuration = (int) value;
        }
        
        private void OnSetCountChanged(float value)
        {
            GameManager.Instance.SetCountToWinAMatch = (int) value;
        }
    }
}