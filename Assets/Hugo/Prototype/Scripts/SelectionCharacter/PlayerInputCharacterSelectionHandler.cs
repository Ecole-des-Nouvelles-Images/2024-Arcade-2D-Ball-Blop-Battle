using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class PlayerInputCharacterSelectionHandler : MonoBehaviour
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
                playerInput.gameObject.GetComponent<CharacterSelectionController>().IsPlayerOne = true;
            }
            else
            {
                playerInput.gameObject.GetComponent<CharacterSelectionController>().IsPlayerOne = false;
            }
        }
    }
}