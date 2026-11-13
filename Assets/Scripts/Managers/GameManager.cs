using System;
using System.Collections.Generic;
using Bots;
using Player.ScriptableObjects;
using SelectionArena;
using SelectionGameMode;
using UnityEngine;
using Utils;
using Utils.Singletons;

namespace Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [Header("===== SETTINGS =====")]
        public List<PlayerData> Players = new();
        public GameModeData CurrentGameModeData;
        public ArenaData CurrentArenaData;
        public int SetDuration;
        public int SetCountToWinAMatch;
        [Tooltip("Difficulté choisie pour l'IA dans l'écran de sélection (null = celle du prefab BotFSM).")]
        public BotDifficulty BotDifficulty;

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
        /// <summary>DeviceId utilisé pour un joueur contrôlé par l'IA.</summary>
        public const int NoDevice = -1;

        public bool IsAi;
        public int PlayerId;
        public int DeviceId;
        public Blop Blop;
        /// <summary>Difficulté de l'IA (null pour un joueur humain).</summary>
        public BotDifficulty BotDifficulty;

        public PlayerData(bool isAi, int playerId, int deviceId, Blop blop, BotDifficulty botDifficulty = null)
        {
            IsAi = isAi;
            PlayerId = playerId;
            DeviceId = deviceId;
            Blop = blop;
            BotDifficulty = botDifficulty;
        }
    }

    public enum GameMode
    {
        Training,
        Pve,
        Pvp
    }
}