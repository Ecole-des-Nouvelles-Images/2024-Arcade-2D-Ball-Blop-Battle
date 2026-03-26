using UnityEngine;
using Utils;

namespace UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            SceneLoaderManager.Instance.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            SceneLoaderManager.Instance.QuitGame();
        }
    }
}