using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bots;
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
        [SerializeField] private GameObject _botFsm;
        
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
                GameObject goPlayer = Instantiate(_player, transform);
                goPlayer.name = goPlayer.name + "_" + playerData.PlayerId;

                PlayerController playerController = goPlayer.GetComponent<PlayerController>();

                playerController.SetUp(playerData.Blop, playerData.PlayerId);
                goPlayer.transform.position = _playersStartPos[playerData.PlayerId - 1];

                _goPlayers.Add(goPlayer);

                // Un joueur dont la manette a été débranchée depuis la sélection est repris par l'IA
                InputDevice device = playerData.IsAi ? null : UnityEngine.InputSystem.InputSystem.GetDeviceById(playerData.DeviceId);

                if (device != null)
                {
                    PlayerInputHandler playerInputHandler = goPlayer.GetComponent<PlayerInputHandler>();

                    GameObject goPlayerInput = Instantiate(_playerInput, transform);
                    goPlayerInput.name = goPlayerInput.name + "_" + playerData.PlayerId;

                    PlayerInput playerInput = goPlayerInput.GetComponent<PlayerInput>();

                    playerInputHandler.Setup(playerInput);
                    SwitchControlScheme(playerInput, device);
                }
                else
                {
                    GameObject goBotFsm = Instantiate(_botFsm, transform);
                    goBotFsm.name = goBotFsm.name + "_" + playerData.PlayerId;
                    
                    BotFsm botFsm = goBotFsm.GetComponent<BotFsm>();

                    botFsm.Setup(playerController, playerData.BotDifficulty);
                }
            }
        }
        
        private static void SwitchControlScheme(PlayerInput playerInput, InputDevice device)
        {
            if (device is Keyboard keyboard && Mouse.current != null)
            {
                playerInput.SwitchCurrentControlScheme(keyboard, Mouse.current);
            }
            else
            {
                playerInput.SwitchCurrentControlScheme(device);
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