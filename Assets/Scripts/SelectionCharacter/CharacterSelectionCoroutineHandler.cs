using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Singletons;

namespace SelectionCharacter
{
    public class CharacterSelectionCoroutineHandler : MonoBehaviourSingleton<CharacterSelectionCoroutineHandler>
    {
        private Image _panelTimerImage;
        
        // Coroutine
        private Coroutine _loadSceneCoroutine;
        
        [Header("Settings")]
        [SerializeField] private float _delay;

        [Header("Text Timer")]
        [SerializeField] private GameObject _panelBackGround;
        [SerializeField] private GameObject _panelTimer;
        [SerializeField] private List<Sprite> _countdownSprites;

        private void Awake()
        {
            _panelTimerImage = _panelTimer.GetComponent<Image>();
        }

        private void Update()
        {
            if (GameManager.Instance.Players.Count == 2 && !GameManager.HasGameLoaded)
            {
                GameManager.HasGameLoaded = true;
                _panelTimer.SetActive(true);
                _loadSceneCoroutine = StartCoroutine(LoadSceneWithDelay());
            }
            else if (GameManager.Instance.Players.Count < 2)
            {
                _panelTimer.SetActive(false);
                CancelLoadScene();
            }
        }

        private IEnumerator LoadSceneWithDelay()
        {
            _panelBackGround.SetActive(true);
            int index = 0;

            while (index < _countdownSprites.Count)
            {
                _panelTimerImage.sprite = _countdownSprites[index];
                index++;
                yield return new WaitForSeconds(_delay);
            }
            
            yield return new WaitForSeconds(_delay);

            SceneLoaderManager.Instance.LoadSceneAnimation("LabArena");
        }

        private void CancelLoadScene()
        {
            if (_loadSceneCoroutine != null)
            {
                _panelBackGround.SetActive(false);
                StopCoroutine(_loadSceneCoroutine);
                _loadSceneCoroutine = null;
                GameManager.HasGameLoaded = false;
            }
        }
    }
}
