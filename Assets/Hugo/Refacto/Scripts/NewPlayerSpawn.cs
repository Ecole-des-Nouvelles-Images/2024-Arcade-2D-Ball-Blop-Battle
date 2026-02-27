using System.Collections;
using Hugo.Prototype.Scripts.Game;
using Int.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Refacto.Scripts
{
    public class NewPlayerSpawn : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _playerOne;
        [SerializeField] private PlayerInput _playerOneInput;
        [SerializeField] private GameObject _playerTwo;
        [SerializeField] private PlayerInput _playerTwoInput;
        
        [Header("Settings")]
        [SerializeField] private Vector2 _playerOneStartPos;
        [SerializeField] private Vector2 _playerTwoStartPos;
        
        private NewBlopController _playerOneController;
        private NewBlopController _playerTwoController;
        
        private void Start()
        {
            _playerOneController = _playerOne.GetComponent<NewBlopController>();
            _playerTwoController = _playerTwo.GetComponent<NewBlopController>();

            foreach (var gamepad in Gamepad.all)
            {
                if (GameManager.Instance.DevicesID.Contains(gamepad.deviceId))
                {
                    int index = GameManager.Instance.DevicesID.IndexOf(gamepad.deviceId);

                    if (index == 0)
                    {
                        _playerOneInput.SwitchCurrentControlScheme(gamepad);
                        _playerOneController.SetUp(GameManager.Instance.FirstBlopScriptableObject, 1);
                    }
                    else if (index == 1)
                    {
                        _playerTwoInput.SwitchCurrentControlScheme(gamepad);
                        _playerTwoController.SetUp(GameManager.Instance.SecondBlopScriptableObject, 2);
                    }
                }
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

        #region === EVENTS ===

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
