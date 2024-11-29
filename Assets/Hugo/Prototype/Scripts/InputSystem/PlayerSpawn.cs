using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.InputSystem
{
    public class PlayerSpawn : MonoBehaviour
    {
        private PlayerInputManager _playerInputHandler;
        
        [Header("GameManager")]
        [SerializeField] private GameManager _gameManager;
        
        [Header("Spawn Points")]
        [SerializeField] private Vector2 _firstSpawnPoints;
        [SerializeField] private Vector2 _secondSpawnPoints;

        private void Awake()
        {
            _playerInputHandler = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            _playerInputHandler.onPlayerJoined += OnPlayerJoined;
        }

        private void OnDisable()
        {
            _playerInputHandler.onPlayerJoined -= OnPlayerJoined;
        }
        
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            if (_gameManager.FirstPlayerGameObject == null)
            {
                _gameManager.FirstPlayerGameObject = playerInput.gameObject;
                playerInput.gameObject.transform.position = _firstSpawnPoints;
                
                playerInput.gameObject.GetComponent<PlayerNumberTouchBallManager>().IsPlayerOne = true;
            }
            else
            {
                _gameManager.SecondPlayerGameObject = playerInput.gameObject;
                playerInput.gameObject.transform.position = _secondSpawnPoints;
                
                playerInput.gameObject.GetComponent<PlayerNumberTouchBallManager>().IsPlayerOne = false;
            }
        }
    }
}
