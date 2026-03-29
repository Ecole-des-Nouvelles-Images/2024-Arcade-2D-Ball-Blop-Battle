using System.Collections;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace InputSystem
{
    public class PlayerSpawnHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _playerOne;
        [SerializeField] private PlayerInput _playerOneInput;
        [SerializeField] private GameObject _playerTwo;
        [SerializeField] private PlayerInput _playerTwoInput;
        
        [Header("Settings")]
        [SerializeField] private Vector2 _playerOneStartPos;
        [SerializeField] private Vector2 _playerTwoStartPos;
        
        private PlayerController _playerOneController;
        private PlayerController _playerTwoController;
        
        private void Start()
        {
            _playerOneController = _playerOne.GetComponent<PlayerController>();
            _playerTwoController = _playerTwo.GetComponent<PlayerController>();

            if (GameManager.Instance.Players.Count > 0)
            {
                foreach (var gamepad in Gamepad.all)
                {
                    PlayerData player = GameManager.Instance.Players.Find(p => p.DeviceId == gamepad.deviceId);

                    if (player == null) return;
                    
                    if (player.PlayerId == 1)
                    {
                        _playerOneInput.SwitchCurrentControlScheme(gamepad);
                        _playerOneController.SetUp(player.Blop, player.PlayerId);
                    }
                    else if (player.PlayerId == 2)
                    {
                        _playerTwoInput.SwitchCurrentControlScheme(gamepad);
                        _playerTwoController.SetUp(player.Blop, player.PlayerId);
                    }
                }
            }
            else
            {
                _playerOneInput.SwitchCurrentControlScheme(Gamepad.all[0]);
                _playerOneController.DebugSetUp(1);
                
                _playerTwoInput.SwitchCurrentControlScheme(Gamepad.all[1]);
                _playerTwoController.DebugSetUp(2);
            }
        }
        
        private IEnumerator PlayerRespawn(int playerId)
        {
            yield return new WaitForSeconds(1f);

            if (playerId == 1)
            {
                _playerOne.transform.position = _playerOneStartPos;
                _playerOne.SetActive(true);
            }
            else if (playerId == 2)
            {
                _playerTwo.transform.position = _playerTwoStartPos;
                _playerTwo.SetActive(true);
            }
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