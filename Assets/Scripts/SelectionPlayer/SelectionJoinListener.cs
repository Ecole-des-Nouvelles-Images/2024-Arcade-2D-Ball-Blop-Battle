using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace SelectionPlayer
{
    /// <summary>
    /// Écoute l'action "UI/Submit" sur tous les devices pour détecter qui veut rejoindre la partie.
    /// Les devices ayant appuyé depuis le dernier appel sont récupérés via <see cref="ConsumeJoinRequests"/>.
    /// </summary>
    public class SelectionJoinListener : IDisposable
    {
        private readonly InputActionAsset _actions;
        private readonly InputAction _submit;
        private readonly List<InputDevice> _pendingDevices = new();
        private readonly List<InputDevice> _consumedDevices = new();

        public SelectionJoinListener(InputActionAsset actionsTemplate)
        {
            _actions = Object.Instantiate(actionsTemplate);

            InputActionMap map = _actions.FindActionMap(SelectionInput.ActionMapName, throwIfNotFound: true);
            _submit = map.FindAction(SelectionInput.SubmitActionName, throwIfNotFound: true);
            _submit.performed += OnSubmitPerformed;
            _submit.Enable();
        }

        /// <summary>Renvoie les devices ayant appuyé sur "valider" depuis le dernier appel.</summary>
        public IReadOnlyList<InputDevice> ConsumeJoinRequests()
        {
            _consumedDevices.Clear();
            _consumedDevices.AddRange(_pendingDevices);
            _pendingDevices.Clear();
            return _consumedDevices;
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            InputDevice device = context.control.device;
            if (!_pendingDevices.Contains(device)) _pendingDevices.Add(device);
        }

        public void Dispose()
        {
            if (!_actions) return;

            _submit.performed -= OnSubmitPerformed;
            _actions.Disable();
            Object.Destroy(_actions);
        }
    }
}
