using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SelectionBlop
{
    public class PlayerInputBlopSelectionHandler : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;

        [Header("References")]
        [SerializeField] private List<GameObject> _p1Visuals;
        [SerializeField] private List<GameObject> _p2Visuals;

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
            var character = playerInput.gameObject.GetComponent<BlopSelectionController>();
            bool isP1 = playerInput.playerIndex == 0;
            
            character.IsPlayerOne = isP1;
            
            List<GameObject> targets = isP1 ? _p1Visuals : _p2Visuals;
            foreach (GameObject target in targets)
            {
                target.SetActive(false);
            }
        }
    }
}