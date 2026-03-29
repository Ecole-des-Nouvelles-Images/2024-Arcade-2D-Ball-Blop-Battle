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

        [Header("===== PREFABS =====")]
        [SerializeField] private GameObject _arenaButtonPrefab;
        
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
            SceneLoaderManager.Instance.LoadSceneAnimation(arenaName);
        }
    }
}