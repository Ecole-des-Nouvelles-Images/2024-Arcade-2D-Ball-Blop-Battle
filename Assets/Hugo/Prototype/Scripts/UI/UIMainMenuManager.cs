using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

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
