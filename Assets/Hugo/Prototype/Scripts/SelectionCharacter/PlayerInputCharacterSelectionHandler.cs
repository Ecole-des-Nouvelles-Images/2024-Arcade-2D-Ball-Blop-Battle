using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class PlayerInputCharacterSelectionHandler : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;

        [Header("References")]
        [SerializeField] private GameObject _textJoinPlayerOne;
        [SerializeField] private GameObject _imageJoinPlayerOne;
        [SerializeField] private GameObject _textJoinPlayerTwo;
        [SerializeField] private GameObject _imageJoinPlayerTwo;

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
                _textJoinPlayerOne.SetActive(false);
                _imageJoinPlayerOne.SetActive(false);
            }
            else
            {
                playerInput.gameObject.GetComponent<CharacterSelectionController>().IsPlayerOne = false;
                _textJoinPlayerTwo.SetActive(false);
                _imageJoinPlayerTwo.SetActive(false);
            }
        }
    }
}