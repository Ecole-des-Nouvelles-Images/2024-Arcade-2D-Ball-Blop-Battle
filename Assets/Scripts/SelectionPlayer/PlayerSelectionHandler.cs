using System.Collections;
using System.Collections.Generic;
using Bots;
using Managers;
using SelectionGameMode;
using Sounds;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace SelectionPlayer
{
    /// <summary>
    /// Écran unique de sélection des joueurs et des Blops.
    /// <list type="bullet">
    /// <item>1 manette branchée : P1 contre une IA. P1 choisit son Blop, puis celui de l'IA.</item>
    /// <item>2 manettes branchées en arrivant : P1 et P2 sont directement deux joueurs.</item>
    /// <item>Une manette libre qui appuie sur "valider" remplace l'IA ; si P2 quitte (retour ou débranchement), il redevient une IA.</item>
    /// </list>
    /// Le mode de jeu (PvE / PvP) est déduit de la présence de l'IA au lancement.
    /// </summary>
    public class PlayerSelectionHandler : MonoBehaviour
    {
        [Header("===== INPUTS =====")]
        [Tooltip("Asset d'actions dont le map \"UI\" (Move, Submit, Cancel, Random) pilote la sélection.")]
        [SerializeField] private InputActionAsset _inputActions;

        [Header("===== REFERENCES =====")]
        [SerializeField] private List<BlopPortrait> _portraits;
        [SerializeField] private GridLayoutGroup _portraitsGrid;
        [SerializeField] private SelectionSlotView _playerOneView;
        [SerializeField] private SelectionSlotView _playerTwoView;
        [SerializeField] private MatchSettingsHandler _matchSettings;
        [SerializeField] private AudioSource _audioSource;

        [Header("===== SETTINGS =====")]
        [SerializeField] private float _launchDelay = 1f;
        [SerializeField] private string _nextSceneName = "SC_ArenaSelection";
        [SerializeField] private string _previousSceneName = "SC_MainMenu";

        private GameManager _gameManager;
        private List<GameModeData> _gameModes;

        private SelectionSlot _playerOne;
        private SelectionSlot _playerTwo;
        private SelectionJoinListener _joinListener;

        private Coroutine _launchCoroutine;
        private bool _isLeavingScene;

        #region ===== UNITY =====

        private void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.ResetGameState();
            _gameModes = ResourceLoader.GetAllScriptables<GameModeData>("GameModes");

            // La navigation UI est gérée ici, par joueur : on évite que l'EventSystem déplace aussi la sélection
            if (EventSystem.current) EventSystem.current.sendNavigationEvents = false;

            _playerOne = new SelectionSlot(1, canBeAi: false, cursorIndex: 0);
            _playerTwo = new SelectionSlot(2, canBeAi: true, cursorIndex: Mathf.Min(1, _portraits.Count - 1));
            _joinListener = new SelectionJoinListener(_inputActions);

            AssignStartingDevices();
            RefreshViews();
        }

        private void OnEnable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            UnityEngine.InputSystem.InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDestroy()
        {
            _joinListener?.Dispose();
            _playerOne?.RemoveInput();
            _playerTwo?.RemoveInput();
        }

        private void Update()
        {
            if (_isLeavingScene) return;

            HandleJoins();
            HandlePlayerOneInput();
            HandlePlayerTwoInput();
            UpdateLaunchState();
        }

        #endregion

        #region ===== DEVICES =====

        private void AssignStartingDevices()
        {
            var gamepads = Gamepad.all;

            if (gamepads.Count > 0) _playerOne.AssignInput(CreateInput(gamepads[0]));
            else if (Keyboard.current != null) _playerOne.AssignInput(CreateInput(Keyboard.current));

            if (gamepads.Count > 1) _playerTwo.AssignInput(CreateInput(gamepads[1]));
        }

        private SelectionInput CreateInput(InputDevice device) => new(_inputActions, device);

        /// <summary>Un device non utilisé qui déclenche "UI/Submit" prend la place libre.</summary>
        private void HandleJoins()
        {
            foreach (InputDevice device in _joinListener.ConsumeJoinRequests())
            {
                if (IsDeviceAssigned(device)) continue;

                if (!_playerOne.HasDevice) Join(_playerOne, device);
                else if (!_playerTwo.HasDevice) Join(_playerTwo, device);
            }
        }

        private bool IsDeviceAssigned(InputDevice device) => _playerOne.Owns(device) || _playerTwo.Owns(device);

        private void Join(SelectionSlot slot, InputDevice device)
        {
            // Le joueur qui remplace l'IA choisit lui-même son Blop
            slot.Unconfirm();
            slot.AssignInput(CreateInput(device));
            PlayClip(AudioStock.Instance ? AudioStock.Instance.SlimeButtonClip : null);
            RefreshViews();
        }

        private void Leave(SelectionSlot slot)
        {
            slot.RemoveInput();
            slot.Unconfirm();

            if (slot == _playerOne)
            {
                _playerTwo.Unconfirm();
                _matchSettings.Unfocus();
            }

            RefreshViews();
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change != InputDeviceChange.Removed && change != InputDeviceChange.Disconnected) return;
            if (_playerOne == null || _isLeavingScene) return;

            if (_playerOne.Owns(device)) Leave(_playerOne);
            if (_playerTwo.Owns(device)) Leave(_playerTwo);
        }

        #endregion

        #region ===== PLAYER INPUTS =====

        private void HandlePlayerOneInput()
        {
            SelectionInput input = _playerOne.Input;
            if (input == null || _playerOne.JoinedThisFrame) return;

            if (_matchSettings.IsFocused)
            {
                HandleSettingsInput(input);
                return;
            }

            // P1 choisit son Blop, puis celui de l'IA
            if (!_playerOne.IsConfirmed)
            {
                HandleChoosingInput(input, _playerOne, canReachSettings: true);
            }
            else if (_playerTwo.IsAi && !_playerTwo.IsConfirmed)
            {
                HandleChoosingInput(input, _playerTwo, canReachSettings: false);
            }

            if (input.CancelPressed) HandlePlayerOneCancel();
        }

        private void HandlePlayerOneCancel()
        {
            if (_playerTwo.IsAi && _playerTwo.IsConfirmed)
            {
                _playerTwo.Unconfirm();
            }
            else if (_playerOne.IsConfirmed)
            {
                _playerOne.Unconfirm();
                if (_playerTwo.IsAi) _playerTwo.Unconfirm();
            }
            else
            {
                _isLeavingScene = true;
                _gameManager.ResetGameState();
                SceneLoaderManager.Instance.LoadScene(_previousSceneName);
                return;
            }

            RefreshViews();
        }

        private void HandlePlayerTwoInput()
        {
            SelectionInput input = _playerTwo.Input;
            if (input == null || _playerTwo.JoinedThisFrame) return;

            if (!_playerTwo.IsConfirmed)
            {
                HandleChoosingInput(input, _playerTwo, canReachSettings: false);

                // Retour sans Blop validé : P2 quitte et redevient une IA
                if (input.CancelPressed) Leave(_playerTwo);
            }
            else if (input.CancelPressed)
            {
                _playerTwo.Unconfirm();
                RefreshViews();
            }
        }

        private void HandleChoosingInput(SelectionInput input, SelectionSlot slot, bool canReachSettings)
        {
            Vector2Int direction = input.ReadNavigation();
            if (direction != Vector2Int.zero) MoveCursor(slot, direction, canReachSettings);

            if (input.RandomPressed)
            {
                slot.CursorIndex = Random.Range(0, _portraits.Count);
                Confirm(slot);
            }
            else if (input.SubmitPressed)
            {
                Confirm(slot);
            }
        }

        private void HandleSettingsInput(SelectionInput input)
        {
            Vector2Int direction = input.ReadNavigation();

            if (direction.y != 0 && !_matchSettings.MoveFocus(-direction.y))
            {
                _matchSettings.Unfocus();
            }
            else if (direction.x != 0)
            {
                _matchSettings.ChangeFocusedValue(direction.x);
            }

            if (input.SubmitPressed || input.CancelPressed)
            {
                _matchSettings.Unfocus();
            }
        }

        #endregion

        #region ===== SELECTION =====

        private void MoveCursor(SelectionSlot slot, Vector2Int direction, bool canReachSettings)
        {
            int count = _portraits.Count;
            int columns = GetColumnCount();
            int rows = Mathf.CeilToInt(count / (float) columns);

            int column = slot.CursorIndex % columns;
            int row = slot.CursorIndex / columns;

            column = (column + direction.x + columns) % columns;
            row -= direction.y;

            if (row >= rows && canReachSettings)
            {
                _matchSettings.Focus();
                PlayClip(AudioStock.Instance ? AudioStock.Instance.ClassicButtonClip : null);
                return;
            }

            row = (row + rows) % rows;
            slot.CursorIndex = Mathf.Min(row * columns + column, count - 1);

            PlayClip(AudioStock.Instance ? AudioStock.Instance.ClassicButtonClip : null);
            RefreshViews();
        }

        private int GetColumnCount()
        {
            int count = _portraits.Count;
            if (!_portraitsGrid) return count;

            return _portraitsGrid.constraint switch
            {
                GridLayoutGroup.Constraint.FixedColumnCount => Mathf.Max(1, _portraitsGrid.constraintCount),
                GridLayoutGroup.Constraint.FixedRowCount => Mathf.Max(1, Mathf.CeilToInt(count / (float) _portraitsGrid.constraintCount)),
                _ => count
            };
        }

        private void Confirm(SelectionSlot slot)
        {
            slot.Confirm(_portraits[slot.CursorIndex].Blop);
            GetView(slot).PlayConfirmFeedback();
            PlayClip(AudioStock.Instance ? AudioStock.Instance.SlimeButtonClip : null);
            RefreshViews();
        }

        private SelectionSlotView GetView(SelectionSlot slot) => slot == _playerOne ? _playerOneView : _playerTwoView;

        private void RefreshViews()
        {
            bool playerOneControlsAi = _playerOne.HasDevice && _playerOne.IsConfirmed;

            _playerOneView.Refresh(_playerOne, _portraits[_playerOne.CursorIndex],
                isControlled: _playerOne.HasDevice && !_playerOne.IsConfirmed);

            _playerTwoView.Refresh(_playerTwo, _portraits[_playerTwo.CursorIndex],
                isControlled: !_playerTwo.IsConfirmed && (_playerTwo.HasDevice || playerOneControlsAi));

            // La difficulté n'a de sens que contre l'IA
            _matchSettings.SetDifficultyVisible(_playerTwo.IsAi);
        }

        #endregion

        #region ===== LAUNCH =====

        private void UpdateLaunchState()
        {
            bool isReady = _playerOne.HasDevice && _playerOne.IsConfirmed && _playerTwo.IsConfirmed;

            if (isReady && _launchCoroutine == null)
            {
                _launchCoroutine = StartCoroutine(LaunchWithDelay());
            }
            else if (!isReady && _launchCoroutine != null)
            {
                StopCoroutine(_launchCoroutine);
                _launchCoroutine = null;
            }
        }

        private IEnumerator LaunchWithDelay()
        {
            yield return new WaitForSeconds(_launchDelay);

            _isLeavingScene = true;
            SaveSelection();
            SceneLoaderManager.Instance.LoadScene(_nextSceneName);
        }

        private void SaveSelection()
        {
            _gameManager.Players.Clear();
            _gameManager.Players.Add(CreatePlayerData(_playerOne));
            _gameManager.Players.Add(CreatePlayerData(_playerTwo));

            GameMode gameMode = _playerTwo.IsAi ? GameMode.Pve : GameMode.Pvp;
            _gameManager.CurrentGameModeData = _gameModes.Find(g => g.GameModeType == gameMode);
        }

        private PlayerData CreatePlayerData(SelectionSlot slot)
        {
            int deviceId = slot.IsAi ? PlayerData.NoDevice : slot.Input.Device.deviceId;
            BotDifficulty botDifficulty = slot.IsAi ? _gameManager.BotDifficulty : null;
            return new PlayerData(slot.IsAi, slot.PlayerId, deviceId, slot.ConfirmedBlop, botDifficulty);
        }

        #endregion

        private void PlayClip(AudioClip clip)
        {
            if (_audioSource && clip) _audioSource.PlayOneShot(clip);
        }
    }
}
