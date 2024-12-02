using Hugo.Prototype.Scripts.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIInGame : MonoBehaviour
    {
        public void Resume()
        {
            GameManager.IsGamePaused = false;
        }
        
        public void Restart()
        {
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;

            MatchManager.ScorePlayerOne = 0;
            MatchManager.ScorePlayerTwo = 0;
            MatchManager.PlayerOneScoreLast = true;
            MatchManager.CurrentTime = 0;
            MatchManager.IsBallInGame = true;
            
            Time.timeScale = 1;
            
            SceneManager.LoadScene(2);
        }
        
        public void BackMenu()
        {
            ResetParameters();
            SceneManager.LoadScene(0);
        }
        
        public void Quit()
        {
            ResetParameters();
            Application.Quit();
        }

        private void ResetParameters()
        {
            GameManager.FirstPlayerScriptableObject = null;
            GameManager.SecondPlayerScriptableObject = null;
            GameManager.HasGameLoaded = false;
            GameManager.IsGamePaused = false;

            MatchManager.ScorePlayerOne = 0;
            MatchManager.ScorePlayerTwo = 0;
            MatchManager.PlayerOneScoreLast = true;
            MatchManager.CurrentTime = 0;
            MatchManager.IsBallInGame = true;
            
            Time.timeScale = 1;
        }
    }
}
