using System.Collections.Generic;
using Managers;
using UI.Menu;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace SelectionGameMode
{
    public class GameModeSelectionHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private UIFirstSelectedButton _uiFirstSelectedButton;
        
        [Header("===== SLIDERS =====")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Slider _setCountSlider;
        
        [Header("===== GAME MODES =====")]
        [SerializeField] private GameObject _gameModeButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        
        private List<GameModeData> _gameModesData;
        
        public void GameModeSelected(GameModeData gameModeData)
        {
            GameManager.Instance.CurrentGameModeData = gameModeData;
            SceneLoaderManager.Instance.LoadScene("SC_BlopSelection");
            
            // if (gameMode == GameMode.Training)
            // {
            //     Debug.Log(" SOON ");
            // }
            // else if (gameMode == GameMode.PvE)
            // {
            //     SceneLoaderManager.Instance.LoadScene("SC_BlopSelection");
            // }
            // else if (gameMode == GameMode.PvP)
            // {
            //     SceneLoaderManager.Instance.LoadScene("SC_BlopSelection");
            // }
        }
        
        private void Start()
        {
            // GameModes
            _gameModesData = ResourceLoader.GetAllScriptables<GameModeData>("GameModes");

            for (var index = 0; index < _gameModesData.Count; index++)
            {
                var gameModeData = _gameModesData[index];
                
                GameObject go = Instantiate(_gameModeButtonPrefab, _buttonsContainer);
                go.GetComponent<GameModeButton>().Setup(gameModeData);
                go.GetComponent<Button>().onClick.AddListener(() => GameModeSelected(gameModeData));

                if (index == 0) _uiFirstSelectedButton.SelectFirstButton(go);
            }
            
            // Sliders
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