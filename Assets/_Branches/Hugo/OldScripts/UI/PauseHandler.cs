using Managers;
using UnityEngine;
using Utils;

namespace _Branches.Hugo.OldScripts.UI
{
    public class PauseHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pausePanel;

        public void Resum()
        {
            EventBus.OnGameResumed?.Invoke();
        }
        
        public void Restart()
        {
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;
            
            Time.timeScale = 1;
            
            SceneLoaderManager.Instance.LoadSceneAnimation("LabArena");
        }
        
        public void BackMenu()
        {
            GameManager.Instance.ResetGameState();
            SceneLoaderManager.Instance.LoadSceneAnimation("MainMenu");
        }
        
        public void Quit()
        {
            GameManager.Instance.ResetGameState();
            Application.Quit();
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnGamePaused += GamePaused;
            EventBus.OnGameResumed += GameResumed;
        }

        private void OnDisable()
        {
            EventBus.OnGamePaused -= GamePaused;
            EventBus.OnGameResumed -= GameResumed;
        }

        private void GamePaused()
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        
        private void GameResumed()
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
        }

        #endregion
    }
}
