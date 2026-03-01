using System;
using Hugo.Refacto.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Settings")]
        public bool InputAreEnable = true;
        [FormerlySerializedAs("_playerInput")] public PlayerInput PlayerInput;
        
        public static event Action<bool> OnInputDeviceChanged;
        
        private NewBlopController _newBlopController;
        
        private bool _isControllerConnected;
        
        private Vector2 _joystickReadValue;
        private float _westButtonReadValue;
        private float _leftButtonReadValue;

        private void Awake()
        {
            _newBlopController = GetComponent<NewBlopController>();
        }

        private void OnEnable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;

            // Bind input actions
            PlayerInput.actions["LeftJoystick"].performed += LeftJoystick;
            PlayerInput.actions["LeftJoystick"].canceled += LeftJoystick;
            PlayerInput.actions["SouthButton"].performed += SouthButton;
            PlayerInput.actions["SouthButton"].canceled += SouthButton;
            PlayerInput.actions["WestButton"].performed += WestButton;
            PlayerInput.actions["WestButton"].canceled += WestButton;
            PlayerInput.actions["EastButton"].performed += EastButton;
            PlayerInput.actions["EastButton"].canceled += EastButton;
            PlayerInput.actions["StartButton"].performed += StartButton;
            PlayerInput.actions["StartButton"].canceled += StartButton;
        }
        
        private void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange -= OnDeviceChange;

            // Unbind input actions
            PlayerInput.actions["LeftJoystick"].performed -= LeftJoystick;
            PlayerInput.actions["LeftJoystick"].canceled -= LeftJoystick;
            PlayerInput.actions["SouthButton"].performed -= SouthButton;
            PlayerInput.actions["SouthButton"].canceled -= SouthButton;
            PlayerInput.actions["WestButton"].performed -= WestButton;
            PlayerInput.actions["WestButton"].canceled -= WestButton;
            PlayerInput.actions["EastButton"].performed -= EastButton;
            PlayerInput.actions["EastButton"].canceled -= EastButton;
            PlayerInput.actions["StartButton"].performed -= StartButton;
            PlayerInput.actions["StartButton"].canceled -= StartButton;
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
            if (InputAreEnable)
            {
                _newBlopController.GetJoystickReadValue(context.ReadValue<Vector2>());
            }
        }
        
        private void SouthButton(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _newBlopController.GetSouthButtonReadValue(context.ReadValue<float>());
            }
        }
        
        private void WestButton(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _newBlopController.GetWestButtonReadValue(context.ReadValue<float>());
            }
        }
        
        private void EastButton(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _newBlopController.GetEastButtonReadValue(context.ReadValue<float>());
            }
        }

        private void StartButton(InputAction.CallbackContext context)
        {
            if (InputAreEnable)
            {
                _newBlopController.GetStartButtonReadValue(context.ReadValue<float>());
            }
        }
    }
}