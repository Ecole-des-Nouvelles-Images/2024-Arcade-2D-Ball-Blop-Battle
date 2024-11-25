using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public static event Action<bool> OnInputDeviceChanged;
        
        private PlayerInput _playerInput;
        
        private PlayerController _playerController;
        
        private bool _isControllerConnected;
        
        private Vector2 _joystickReadValue;
        private float _westButtonReadValue;
        private float _leftButtonReadValue;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;

            // Bind input actions
            _playerInput.actions["LeftJoystick"].performed += LeftJoystick;
            _playerInput.actions["LeftJoystick"].canceled += LeftJoystick;
            _playerInput.actions["SouthButton"].performed += SouthButton;
            _playerInput.actions["SouthButton"].canceled += SouthButton;
            _playerInput.actions["LeftButton"].performed += WestButton;
            _playerInput.actions["LeftButton"].canceled += WestButton;
        }
        
        private void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange -= OnDeviceChange;

            // Unbind input actions
            _playerInput.actions["LeftJoystick"].performed -= LeftJoystick;
            _playerInput.actions["LeftJoystick"].canceled -= LeftJoystick;
            _playerInput.actions["SouthButton"].performed -= SouthButton;
            _playerInput.actions["SouthButton"].canceled -= SouthButton;
            _playerInput.actions["LeftButton"].performed -= WestButton;
            _playerInput.actions["LeftButton"].canceled -= WestButton;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added || change == InputDeviceChange.Removed) DetectCurrentInputDevice();
        }

        private void DetectCurrentInputDevice()
        {
            _isControllerConnected = Gamepad.all.Count > 0;
            OnInputDeviceChanged?.Invoke(_isControllerConnected);

            Debug.Log(_isControllerConnected
                ? "Controller connected: Switching to Gamepad controls."
                : "No controller connected: Switching to Keyboard/Mouse controls.");
        }
        
        private void LeftJoystick(InputAction.CallbackContext context)
        {
            _playerController.GetJoystickReadValue(context.ReadValue<Vector2>());
        }
        
        private void WestButton(InputAction.CallbackContext context)
        {
            _playerController.GetWestButtonReadValue(context.ReadValue<float>());
        }

        private void SouthButton(InputAction.CallbackContext context)
        {
            _playerController.GetSouthButtonReadValue(context.ReadValue<float>());
        }
    }
}