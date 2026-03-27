using System;
using System.Collections.Generic;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;
using Utils.Singletons;

namespace Managers
{
    public class GameManager : MonoBehaviourSingletonDontDestroyOnLoad<GameManager>
    {
        [Header("===== SETTINGS =====")]
        public List<PlayerData> Players = new();
        public string GameMode;
        public int SetDuration;
        public int SetCountToWinAMatch;
        public string ArenaSceneName;
        
        [Header("===== GAME STATES =====")]
        public static bool HasGameLoaded = false;
        public static bool IsGamePaused = false;

        public void ResetGameState()
        {
            Players.Clear();
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

    [Serializable]
    public class PlayerData
    {
        public int PlayerId;
        public int DeviceId;
        public Blop Blop;

        public PlayerData(int playerId, int deviceId, Blop blop)
        {
            PlayerId = playerId;
            DeviceId = deviceId;
            Blop = blop;
        }
    }
}