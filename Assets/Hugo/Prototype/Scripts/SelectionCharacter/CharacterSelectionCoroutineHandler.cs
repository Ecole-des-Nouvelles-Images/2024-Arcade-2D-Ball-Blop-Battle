using System;
using System.Collections;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Utils;
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
        [SerializeField] private GameObject _panelBackGround;
        [SerializeField] private GameObject _panelTimer;
        [SerializeField] private Sprite _spriteThree;
        [SerializeField] private Sprite _spriteTwo;
        [SerializeField] private Sprite _spriteOne;
        [SerializeField] private Sprite _spriteGo;

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
            
            _panelBackGround.SetActive(true);

            while (remainingTime > 0f)
            {
                if (Math.Abs(remainingTime - 3f) < 0.2f)
                {
                    _panelTimerImage.sprite = _spriteThree;
                }
                if (Math.Abs(remainingTime - 2f) < 0.2f)
                {
                    _panelTimerImage.sprite = _spriteTwo;
                }
                if (Math.Abs(remainingTime - 1f) < 0.2f)
                {
                    _panelTimerImage.sprite = _spriteOne;
                }
                if (Math.Abs(remainingTime - 0.1f) < 0.1f)
                {
                    _panelTimerImage.sprite = _spriteGo;
                }
                
                yield return null;
                
                remainingTime -= Time.deltaTime;
            }
            
            yield return new WaitForSeconds(delay);

            SceneManager.LoadScene(3);
        }
        
        public void CancelLoadScene()
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
