using _Branches.Hugo.OldScripts.Game;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UIInGame : MonoBehaviour
    {
        // Panels
        private bool _isPanelActive;
        
        [Header("Panels")]
        [SerializeField] private GameObject _pausePanel;

        private void Update()
        {
            if (_pausePanel)
            {
                if (GameManager.IsGamePaused && !_isPanelActive)
                {
                    _pausePanel.SetActive(true);
                    _isPanelActive = true;
                
                    Time.timeScale = 0;
                }

                if (!GameManager.IsGamePaused && _isPanelActive)
                {
                    _pausePanel.SetActive(false);
                    _isPanelActive = false;
                
                    Time.timeScale = 1;
                }
            }
        }

        public void Resum()
        {
            GameManager.IsGamePaused = false;
        }
        
        public void Restart()
        {
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;

            OldMatchManager.ScorePlayerOne = 0;
            OldMatchManager.ScorePlayerTwo = 0;
            OldMatchManager.PlayerOneScoreLast = true;
            OldMatchManager.CurrentTime = 0;
            
            Time.timeScale = 1;
            
            SceneManager.LoadScene(3);
        }
        
        public void BackMenu()
        {
            ResetParameters();
            SceneManager.LoadScene(1);
        }
        
        public void Quit()
        {
            ResetParameters();
            Application.Quit();
        }

        private void ResetParameters()
        {
            GameManager.Instance.FirstBlopScriptableObject = null;
            GameManager.Instance.SecondBlopScriptableObject = null;
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;

            OldMatchManager.ScorePlayerOne = 0;
            OldMatchManager.ScorePlayerTwo = 0;
            OldMatchManager.PlayerOneScoreLast = true;
            OldMatchManager.CurrentTime = 0;
            
            Time.timeScale = 1;
        }
    }
}
