using System;
using System.Collections;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class CharacterSelectionCoroutineHandler : MonoBehaviourSingleton<CharacterSelectionCoroutineHandler>
    {
        private Image _panelTimerImage;
        
        // Coroutine
        private Coroutine _loadSceneCoroutine;

        [Header("Text Timer")]
        [SerializeField] private GameObject _panelTimer;
        [SerializeField] private TextMeshProUGUI _textTimer;

        private void Awake()
        {
            _panelTimerImage = _panelTimer.GetComponent<Image>();
        }

        private void Update()
        {
            if (GameManager.FirstPlayerScriptableObject && GameManager.SecondPlayerScriptableObject && !GameManager.HasGameLoaded)
            {
                GameManager.HasGameLoaded = true;
                _panelTimer.SetActive(true);
                _loadSceneCoroutine = StartCoroutine(LoadSceneWithDelay(3f));
            }

            if (!GameManager.FirstPlayerScriptableObject || !GameManager.SecondPlayerScriptableObject)
            {
                _panelTimer.SetActive(false);
                CancelLoadScene();
            }
        }

        private IEnumerator LoadSceneWithDelay(float delay)
        {
            float remainingTime = delay;

            while (remainingTime > 0)
            {
                _textTimer.text = Mathf.Ceil(remainingTime).ToString();
                
                yield return null;
                
                remainingTime -= Time.deltaTime;
            }

            _textTimer.text = " Go ! ";
            
            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(3);
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
