using Hugo.Prototype.Scripts.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.InputSystem
{
    public class PlayerSpawn : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;
        
        [Header("GameManager")]
        [SerializeField] private GameManager _gameManager;
        
        [Header("Spawn Points")]
        [SerializeField] private Vector2 _firstSpawnPoints;
        [SerializeField] private Vector2 _secondSpawnPoints;

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
            if (_gameManager.FirstPlayerGameObject == null)
            {
                _gameManager.FirstPlayerGameObject = playerInput.gameObject;
                playerInput.gameObject.transform.position = _firstSpawnPoints;
            }
            else
            {
                _gameManager.SecondPlayerGameObject = playerInput.gameObject;
                playerInput.gameObject.transform.position = _secondSpawnPoints;
            }
        }
    }
}
