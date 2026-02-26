using Hugo.Prototype.Scripts.Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Refacto.Scripts
{
    public class NewPlayerSpawn : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject _playerOne;
        [SerializeField] private GameObject _playerTwo;
        
        private PlayerInput _playerOneInput;
        private PlayerInput _playerTwoInput;
        
        private NewBlopController _playerOneController;
        private NewBlopController _playerTwoController;
        
        private void Start()
        {
            _playerOneInput = _playerOne.GetComponent<PlayerInput>();
            _playerTwoInput = _playerTwo.GetComponent<PlayerInput>();
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
    }
}
