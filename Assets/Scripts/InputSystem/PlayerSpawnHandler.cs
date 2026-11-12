using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace InputSystem
{
    public class PlayerSpawnHandler : MonoBehaviour
    {
        [Header("===== PREFABS =====")]
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _playerInput;
        
        [Header("===== SETTINGS =====")]
        [SerializeField] private List<Vector2> _playersStartPos;

        [Header("===== DEBUG =====")]
        private GameManager _gameManager;
        private List<GameObject> _goPlayers = new();
        
        private void Start()
        {
            _gameManager = GameManager.Instance;

            _gameManager.Players = _gameManager.Players.OrderBy(p => p.PlayerId).ToList();
            
            foreach (var playerData in _gameManager.Players)
            {
                if (playerData.IsAi == false)
                {
                    Gamepad gamepad = Gamepad.all.FirstOrDefault(g => g.deviceId == playerData.DeviceId);
                    if (gamepad == null) return;

                    GameObject goPlayer = Instantiate(_player, transform);
                    GameObject goPlayerInput = Instantiate(_playerInput, transform);

                    PlayerController playerController = goPlayer.GetComponent<PlayerController>();
                    PlayerInputHandler playerInputHandler = goPlayer.GetComponent<PlayerInputHandler>();
                    PlayerInput playerInput = goPlayerInput.GetComponent<PlayerInput>();
                
                    playerInputHandler.Setup(playerInput);
                    playerInput.SwitchCurrentControlScheme(gamepad);
                    playerController.SetUp(playerData.Blop, playerData.PlayerId);
                    goPlayer.transform.position = _playersStartPos[playerData.PlayerId - 1];
                
                    _goPlayers.Add(goPlayer);
                }
                else
                {
                    
                }
            }
        }
        
        private IEnumerator PlayerRespawn(int playerId)
        {
            yield return new WaitForSeconds(1f);

            _goPlayers[playerId - 1].transform.position = _playersStartPos[playerId - 1];
            _goPlayers[playerId - 1].SetActive(true);
        }
        
        #region ===== EVENTS =====

        private void OnEnable()
        {
            EventBus.OnPlayerDie += PlayerDie;
        }

        private void PlayerDie(int playerId)
        {
            StartCoroutine(nameof(PlayerRespawn), playerId);
        }
        
        private void OnDisable()
        {
            EventBus.OnPlayerDie -= PlayerDie;
        }

        #endregion
    }
}