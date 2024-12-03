using System.Collections;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class CharacterSelectionCoroutineHandler : MonoBehaviourSingleton<CharacterSelectionCoroutineHandler>
    {
        // Coroutine
        private Coroutine _loadSceneCoroutine;

        private void Update()
        {
            if (GameManager.FirstPlayerScriptableObject && GameManager.SecondPlayerScriptableObject && !GameManager.HasGameLoaded)
            {
                GameManager.HasGameLoaded = true;
                _loadSceneCoroutine = StartCoroutine(LoadSceneWithDelay(3f));
            }

            if (!GameManager.FirstPlayerScriptableObject || !GameManager.SecondPlayerScriptableObject)
            {
                CancelLoadScene();
            }
        }

        private IEnumerator LoadSceneWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(2);
        }
        
        public void CancelLoadScene()
        {
            if (_loadSceneCoroutine != null)
            {
                StopCoroutine(_loadSceneCoroutine);
                _loadSceneCoroutine = null;
                GameManager.HasGameLoaded = false;
            }
        }
    }
}
