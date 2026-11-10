using System.Collections;
using Managers;
using UnityEngine;
using Utils;
using Utils.Singletons;

namespace SelectionBlop
{
    public class BlopSelectionHandler : MonoBehaviourSingleton<BlopSelectionHandler>
    {
        [Header("Settings")]
        [SerializeField] private float _delay;

        private GameManager _gameManager;
        
        // Coroutine
        private Coroutine _loadSceneCoroutine;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        private void Update()
        {
            if (_gameManager.CurrentGameModeData.GameModeType == GameMode.Training
                || _gameManager.CurrentGameModeData.GameModeType == GameMode.Pve)
            {
                if (GameManager.Instance.Players.Count == 1 && !GameManager.HasGameLoaded)
                {
                    GameManager.HasGameLoaded = true;
                    _loadSceneCoroutine = StartCoroutine(LoadSceneWithDelay());
                }
                else if (GameManager.Instance.Players.Count < 1)
                {
                    CancelLoadScene();
                }
            }
            else if (_gameManager.CurrentGameModeData.GameModeType == GameMode.Pvp)
            {
                if (GameManager.Instance.Players.Count == 2 && !GameManager.HasGameLoaded)
                {
                    GameManager.HasGameLoaded = true;
                    _loadSceneCoroutine = StartCoroutine(LoadSceneWithDelay());
                }
                else if (GameManager.Instance.Players.Count < 2)
                {
                    CancelLoadScene();
                }
            }
        }

        private IEnumerator LoadSceneWithDelay()
        {
            yield return new WaitForSeconds(_delay);

            SceneLoaderManager.Instance.LoadScene("SC_ArenaSelection");
        }

        private void CancelLoadScene()
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