using System.Collections.Generic;
using Bots;
using Managers;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Utils;

namespace SelectionPlayer
{
    /// <summary>
    /// Réglages du match (durée d'un set, nombre de sets, difficulté de l'IA), affichés sous forme de nombres.
    /// Pilotés au pad par le joueur 1 via <see cref="HandleInput"/> :
    /// gauche / droite passe d'un réglage à l'autre, "valider" entre en édition,
    /// haut / bas change alors la valeur et "valider" ou "retour" sort de l'édition.
    /// La difficulté n'est proposée que lorsque le joueur 2 est une IA.
    /// </summary>
    public class MatchSettingsHandler : MonoBehaviour
    {
        public enum InputResult
        {
            None,
            Moved,
            ValueChanged,
            EditStarted,
            EditEnded,
            Exited
        }

        [Header("===== FIELDS =====")]
        [SerializeField] private MatchSettingField _timerField;
        [SerializeField] private MatchSettingField _setCountField;

        [Header("===== AI DIFFICULTY =====")]
        [SerializeField] private MatchSettingField _difficultyField;
        [Tooltip("Objet masqué quand le joueur 2 est humain (par défaut : le champ lui-même).")]
        [SerializeField] private GameObject _difficultyRow;
        [Tooltip("Dossier Resources contenant les BotDifficulty.")]
        [SerializeField] private string _difficultiesResourcesPath = "BotDifficulty";
        [Tooltip("Difficulté proposée par défaut (position dans la liste triée par Order).")]
        [SerializeField] private int _defaultDifficultyIndex = 1;

        private readonly List<MatchSettingField> _focusOrder = new();
        private List<BotDifficulty> _difficulties = new();
        private bool _isDifficultyVisible = true;
        private int _focusIndex = -1;

        public bool IsFocused => _focusIndex >= 0;
        public bool IsEditing => FocusedField && FocusedField.IsEditing;

        private MatchSettingField FocusedField => IsFocused ? _focusOrder[_focusIndex] : null;
        private bool HasDifficulty => _difficultyField && _difficulties.Count > 0;

        private void Awake()
        {
            if (_difficultyField)
            {
                _difficulties = ResourceLoader.GetAllScriptables<BotDifficulty>(_difficultiesResourcesPath);
                _difficulties.Sort((a, b) => a.Order.CompareTo(b.Order));
            }

            if (!_difficultyRow && _difficultyField) _difficultyRow = _difficultyField.gameObject;

            RebuildFocusOrder();
        }

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;

            _timerField.OnValueChanged += OnTimerChanged;
            _setCountField.OnValueChanged += OnSetCountChanged;

            // Garde les réglages précédents s'ils sont valides, sinon reprend la valeur par défaut du champ
            InitField(_timerField, gameManager.SetDuration, OnTimerChanged);
            InitField(_setCountField, gameManager.SetCountToWinAMatch, OnSetCountChanged);

            SetupDifficultyField();
        }

        private static void InitField(MatchSettingField field, int savedValue, System.Action<int> apply)
        {
            if (savedValue >= field.Min && savedValue <= field.Max) field.SetValueWithoutNotify(savedValue);
            else field.SetValueWithoutNotify(field.Value);

            apply(field.Value);
        }

        #region ===== INPUT =====

        /// <summary>Traite les entrées du joueur 1 quand les réglages ont le focus.</summary>
        public InputResult HandleInput(SelectionInput input)
        {
            if (!IsFocused) return InputResult.None;

            Vector2Int direction = input.ReadNavigation();
            MatchSettingField field = FocusedField;

            if (field.IsEditing)
            {
                if (input.SubmitPressed || input.CancelPressed)
                {
                    field.SetEditing(false);
                    return InputResult.EditEnded;
                }

                return direction.y != 0 && field.Step(direction.y) ? InputResult.ValueChanged : InputResult.None;
            }

            if (input.CancelPressed || direction.y > 0)
            {
                Unfocus();
                return InputResult.Exited;
            }

            if (input.SubmitPressed)
            {
                field.SetEditing(true);
                return InputResult.EditStarted;
            }

            if (direction.x != 0 && MoveFocus(direction.x)) return InputResult.Moved;

            return InputResult.None;
        }

        #endregion

        #region ===== FOCUS =====

        public void Focus()
        {
            SetFocusIndex(0);
        }

        public void Unfocus()
        {
            SetFocusIndex(-1);
        }

        private bool MoveFocus(int delta)
        {
            int next = Mathf.Clamp(_focusIndex + delta, 0, _focusOrder.Count - 1);
            if (next == _focusIndex) return false;

            SetFocusIndex(next);
            return true;
        }

        private void SetFocusIndex(int index)
        {
            if (FocusedField)
            {
                FocusedField.SetEditing(false);
                FocusedField.SetHovered(false);
            }

            _focusIndex = _focusOrder.Count == 0 ? -1 : Mathf.Clamp(index, -1, _focusOrder.Count - 1);

            if (FocusedField) FocusedField.SetHovered(true);
        }

        private void RebuildFocusOrder()
        {
            MatchSettingField focused = FocusedField;

            _focusOrder.Clear();
            if (_timerField) _focusOrder.Add(_timerField);
            if (_setCountField) _focusOrder.Add(_setCountField);
            if (HasDifficulty && _isDifficultyVisible) _focusOrder.Add(_difficultyField);

            if (!focused) return;

            int index = _focusOrder.IndexOf(focused);
            if (index >= 0)
            {
                _focusIndex = index;
                return;
            }

            // Le champ ciblé vient d'être masqué : on se replace sur le dernier champ visible
            focused.SetEditing(false);
            focused.SetHovered(false);
            _focusIndex = -1;
            SetFocusIndex(_focusOrder.Count - 1);
        }

        #endregion

        #region ===== AI DIFFICULTY =====

        /// <summary>Affiche ou masque le réglage de difficulté (visible seulement contre une IA).</summary>
        public void SetDifficultyVisible(bool isVisible)
        {
            if (_isDifficultyVisible == isVisible) return;

            _isDifficultyVisible = isVisible;
            if (_difficultyRow) _difficultyRow.SetActive(isVisible && HasDifficulty);
            RebuildFocusOrder();
        }

        private void SetupDifficultyField()
        {
            if (!HasDifficulty)
            {
                if (_difficultyRow) _difficultyRow.SetActive(false);
                return;
            }

            _difficultyField.SetRange(0, _difficulties.Count - 1);
            _difficultyField.SetFormatter(index => _difficulties[index].GetDisplayName());
            _difficultyField.OnValueChanged += OnDifficultyChanged;

            // Garde le choix précédent (retour au menu, revanche), sinon la difficulté par défaut
            int savedIndex = _difficulties.IndexOf(GameManager.Instance.BotDifficulty);
            if (savedIndex < 0) savedIndex = Mathf.Clamp(_defaultDifficultyIndex, 0, _difficulties.Count - 1);

            _difficultyField.SetValueWithoutNotify(savedIndex);
            OnDifficultyChanged(savedIndex);

            if (_difficultyRow) _difficultyRow.SetActive(_isDifficultyVisible);
        }

        private void OnDifficultyChanged(int index)
        {
            GameManager.Instance.BotDifficulty = _difficulties[Mathf.Clamp(index, 0, _difficulties.Count - 1)];
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale locale)
        {
            if (HasDifficulty) _difficultyField.RefreshText();
        }

        #endregion

        private void OnTimerChanged(int value)
        {
            GameManager.Instance.SetDuration = value;
        }

        private void OnSetCountChanged(int value)
        {
            GameManager.Instance.SetCountToWinAMatch = value;
        }
    }
}
