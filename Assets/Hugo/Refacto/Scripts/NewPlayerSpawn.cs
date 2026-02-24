using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Refacto.Scripts
{
    public class NewPlayerSpawn : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput _playerOne;
        [SerializeField] private PlayerInput _playerTwo;

        private void Start()
        {
            _playerOne.SwitchCurrentControlScheme(Gamepad.all[0]);
            _playerTwo.SwitchCurrentControlScheme(Gamepad.all[1]);
        }
    }
}
