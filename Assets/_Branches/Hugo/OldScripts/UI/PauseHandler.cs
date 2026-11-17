using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace _Branches.Hugo.OldScripts.UI
{
    public class PauseHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _pausePanel;

        private GameManager _gameManager;

        public void Resum()
        {
            EventBus.OnGameResumed?.Invoke();
        }
        
        public void Restart()
        {
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;
            
            Time.timeScale = 1;
            
            SceneLoaderManager.Instance.LoadScenesAdditiveAnimation(_gameManager.CurrentGameModeData.SceneName, _gameManager.CurrentArenaData.ArenaSceneName);
        }
        
        public void BackMenu()
        {
            GameManager.Instance.ResetGameState();
            SceneLoaderManager.Instance.LoadSceneAnimation("SC_MainMenu");
        }
        
        public void Quit()
        {
            GameManager.Instance.ResetGameState();
            Application.Quit();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
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
        
        public string GetAdditiveSceneName()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
        
                if (scene.isLoaded && scene != SceneManager.GetActiveScene())
                {
                    return scene.name;
                }
            }
    
            return null;
        }
    }
}
