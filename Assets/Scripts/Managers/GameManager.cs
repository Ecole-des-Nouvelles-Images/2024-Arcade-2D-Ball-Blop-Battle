using System.Collections.Generic;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;
using Utils.Singletons;

namespace Managers
{
    public class GameManager : MonoBehaviourSingletonDontDestroyOnLoad<GameManager>
    {
        // Players
        public Blop FirstBlopScriptableObject;
        public Blop SecondBlopScriptableObject;
        
        // List DevicesID
        public List<int> DevicesID = new();
        
        // States of Game
        public static bool HasGameLoaded = false;
        public static bool IsGamePaused = false;

        public void ResetGameState()
        {
            Instance.FirstBlopScriptableObject = null;
            Instance.SecondBlopScriptableObject = null;
            HasGameLoaded = false;
            IsGamePaused = false;
            
            Time.timeScale = 1;
        }

        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnGamePaused += GamePaused;
            EventBus.OnGameResumed += GameResumed;
        }

        private void OnDisable()
        {
            EventBus.OnGamePaused -= GamePaused;
            EventBus.OnGameResumed -= GameResumed;
        }

        private void GamePaused()
        {
            IsGamePaused = true;
        }
        
        private void GameResumed()
        {
            IsGamePaused = false;
        }

        #endregion
    }
}