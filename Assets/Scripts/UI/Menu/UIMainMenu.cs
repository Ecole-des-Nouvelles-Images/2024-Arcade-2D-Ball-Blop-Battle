using Managers;
using Player.ScriptableObjects;
using SelectionArena;
using SelectionGameMode;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [Header("===== TRAINING =====")]
        [SerializeField] private Blop _trainingBlop;
        [SerializeField] private GameModeData _trainingGameMode;
        [SerializeField] private ArenaData _trainingArenaData;

        public void ChangeScene(string sceneName)
        {
            SceneLoaderManager.Instance.LoadSceneAnimation(sceneName);
        }

        public void LoadTraining()
        {
            GameManager gameManager = GameManager.Instance;

            InputDevice device = GetLastUsedDevice();
            int deviceId = device != null ? device.deviceId : PlayerData.NoDevice;

            gameManager.ResetGameState();
            gameManager.Players.Add(new PlayerData(false, 1, deviceId, _trainingBlop));
            gameManager.CurrentGameModeData = _trainingGameMode;
            gameManager.CurrentArenaData = _trainingArenaData;

            SceneLoaderManager.Instance.LoadScenesAdditiveAnimation(_trainingGameMode.SceneName, _trainingArenaData.ArenaSceneName);
        }

        public void QuitGame()
        {
            SceneLoaderManager.Instance.QuitGame();
        }

        /// <summary>
        /// Renvoie la manette ou le clavier utilisé le plus récemment (celui qui a validé le bouton Training).
        /// </summary>
        private static InputDevice GetLastUsedDevice()
        {
            InputDevice lastDevice = null;

            // Nom complet requis : le namespace InputSystem du projet masque le type UnityEngine.InputSystem.InputSystem
            foreach (InputDevice device in UnityEngine.InputSystem.InputSystem.devices)
            {
                if (device is not (Gamepad or Keyboard)) continue;
                if (lastDevice == null || device.lastUpdateTime > lastDevice.lastUpdateTime) lastDevice = device;
            }

            return lastDevice;
        }
    }
}
