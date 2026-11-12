using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace SelectionPlayer
{
    /// <summary>
    /// Entrées d'un joueur sur l'écran de sélection, lues depuis le map "UI" de l'asset d'actions.
    /// Chaque joueur a sa propre copie de l'asset, restreinte à son device : les bindings
    /// sont définis une seule fois dans l'asset (Move, Submit, Cancel, Random).
    /// </summary>
    public class SelectionInput : IDisposable
    {
        public const string ActionMapName = "UI";
        public const string MoveActionName = "Move";
        public const string SubmitActionName = "Submit";
        public const string CancelActionName = "Cancel";
        public const string RandomActionName = "Random";

        private const float DeadZone = 0.5f;
        private const float RepeatDelay = 0.35f;
        private const float RepeatRate = 0.12f;

        public InputDevice Device { get; }

        private readonly InputActionAsset _actions;
        private readonly InputAction _move;
        private readonly InputAction _submit;
        private readonly InputAction _cancel;
        private readonly InputAction _random;

        private Vector2Int _heldDirection;
        private float _nextRepeatTime;

        public SelectionInput(InputActionAsset actionsTemplate, InputDevice device)
        {
            Device = device;

            _actions = Object.Instantiate(actionsTemplate);
            _actions.devices = new[] { device };

            InputActionMap map = _actions.FindActionMap(ActionMapName, throwIfNotFound: true);
            _move = map.FindAction(MoveActionName, throwIfNotFound: true);
            _submit = map.FindAction(SubmitActionName, throwIfNotFound: true);
            _cancel = map.FindAction(CancelActionName, throwIfNotFound: true);
            _random = map.FindAction(RandomActionName, throwIfNotFound: true);

            map.Enable();
        }

        public bool SubmitPressed => _submit.WasPressedThisFrame();
        public bool CancelPressed => _cancel.WasPressedThisFrame();
        public bool RandomPressed => _random.WasPressedThisFrame();

        /// <summary>
        /// Renvoie une direction (une seule composante non nulle) au moment de l'appui,
        /// puis de façon répétée tant que la direction est maintenue. À appeler à chaque frame.
        /// </summary>
        public Vector2Int ReadNavigation()
        {
            Vector2 raw = _move.ReadValue<Vector2>();
            Vector2Int direction = Vector2Int.zero;

            if (Mathf.Abs(raw.x) >= DeadZone && Mathf.Abs(raw.x) >= Mathf.Abs(raw.y))
            {
                direction.x = (int) Mathf.Sign(raw.x);
            }
            else if (Mathf.Abs(raw.y) >= DeadZone)
            {
                direction.y = (int) Mathf.Sign(raw.y);
            }

            if (direction == Vector2Int.zero)
            {
                _heldDirection = Vector2Int.zero;
                return Vector2Int.zero;
            }

            if (direction != _heldDirection)
            {
                _heldDirection = direction;
                _nextRepeatTime = Time.unscaledTime + RepeatDelay;
                return direction;
            }

            if (Time.unscaledTime >= _nextRepeatTime)
            {
                _nextRepeatTime = Time.unscaledTime + RepeatRate;
                return direction;
            }

            return Vector2Int.zero;
        }

        public void Dispose()
        {
            if (!_actions) return;

            _actions.Disable();
            Object.Destroy(_actions);
        }
    }
}
