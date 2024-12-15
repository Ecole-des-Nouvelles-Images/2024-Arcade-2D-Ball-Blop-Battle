using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        public void ChangeScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
