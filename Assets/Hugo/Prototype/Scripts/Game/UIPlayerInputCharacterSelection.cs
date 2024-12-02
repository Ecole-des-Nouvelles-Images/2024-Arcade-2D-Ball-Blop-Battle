using Hugo.Prototype.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.Game
{
    public class UIPlayerInputCharacterSelection : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;

        private void Awake()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
        }

        private void OnDisable()
        {
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            if (playerInput.playerIndex == 0)
            {
                playerInput.gameObject.GetComponent<UICharacterSelection>().IsPlayerOne = true;
            }
            else
            {
                playerInput.gameObject.GetComponent<UICharacterSelection>().IsPlayerOne = false;
            }
        }
    }
}