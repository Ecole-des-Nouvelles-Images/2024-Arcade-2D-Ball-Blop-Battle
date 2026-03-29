using System.Collections;
using System.Collections.Generic;
using UI.Menu;
using UnityEngine;
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
            _arenas = ResourceLoader.GetAllScriptables<ArenaData>("Arenas");

            for (var index = 0; index < _arenas.Count; index++)
            {
                var arena = _arenas[index];
                
                GameObject go = Instantiate(_arenaButtonPrefab, _buttonsContainer);
                go.GetComponent<ArenaButton>().Setup(arena.ArenaName, arena.ArenaVisual);
                go.GetComponent<Button>().onClick.AddListener(() => SelectArena(arena.ArenaSceneName));

                if (index == 0) _uiFirstSelectedButton.SelectFirstButton(go);
            }
        }
        
        private void SelectArena(string arenaName)
        {
            // SceneLoaderManager.Instance.LoadSceneAnimation(arenaName);
            StartCoroutine(LoadSceneWithDelay(arenaName));
        }
        
        private IEnumerator LoadSceneWithDelay(string arenaName)
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

            SceneLoaderManager.Instance.LoadScene(arenaName);
        }
    }
}