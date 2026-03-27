using UnityEngine;
using Utils;

namespace SelectionArena
{
    public class ArenaSelectionHandler : MonoBehaviour
    {
        public void SelectArena(string arenaName)
        {
            SceneLoaderManager.Instance.LoadSceneAnimation(arenaName);
        }
    }
}