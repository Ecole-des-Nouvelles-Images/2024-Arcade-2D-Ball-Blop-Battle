using System.Collections;
using System.Collections.Generic;
using Managers;
using UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace SelectionArena
{
    public class ArenaSelectionHandler : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private UIFirstSelectedButton _uiFirstSelectedButton;
        [SerializeField] private Image _imageTimer;

        [Header("===== PREFABS =====")]
        [SerializeField] private GameObject _arenaButtonPrefab;
        [SerializeField] private List<Sprite> _countdownSprites;
        
        private List<ArenaData> _arenas;

        private void Start()
        {
            // Load Arenas
            _arenas = ResourceLoader.GetAllScriptables<ArenaData>("Arenas");

            for (var index = 0; index < _arenas.Count; index++)
            {
                var arenaData = _arenas[index];
                
                GameObject go = Instantiate(_arenaButtonPrefab, _buttonsContainer);
                go.GetComponent<ArenaButton>().Setup(arenaData.ArenaName, arenaData.ArenaVisual);
                go.GetComponent<Button>().onClick.AddListener(() => SelectArena(arenaData));

                if (index == 0) _uiFirstSelectedButton.SelectFirstButton(go);
            }
        }
        
        private void SelectArena(ArenaData arenaData)
        {
            GameManager.Instance.CurrentArenaData = arenaData;
            EventSystem.current.enabled = false;
            StartCoroutine(LoadSceneWithDelay(arenaData.ArenaSceneName));
        }
        
        private IEnumerator LoadSceneWithDelay(string arenaSceneName)
        {
            var color = _imageTimer.color;
            color.a = 1f;
            _imageTimer.color = color;
            
            int index = 0;

            while (index < _countdownSprites.Count)
            {
                _imageTimer.sprite = _countdownSprites[index];
                index++;
                yield return new WaitForSeconds(1f);
            }
            
            yield return new WaitForSeconds(1f);

            SceneLoaderManager.Instance.LoadScenesAdditiveAnimation(GameManager.Instance.CurrentGameModeData.SceneName, arenaSceneName);
        }
    }
}