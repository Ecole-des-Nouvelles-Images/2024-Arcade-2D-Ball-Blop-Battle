using UnityEngine;
using Utils;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UIMainMenuManager : MonoBehaviour
    {
        public void ChangeScene(int index)
        {
            SceneLoaderManager.Instance.LoadScene(index);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
