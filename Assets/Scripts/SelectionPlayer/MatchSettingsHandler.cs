using System.Collections.Generic;
using Bots;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Utils;

namespace SelectionPlayer
{
    /// <summary>
    /// Réglages du match (durée d'un set, nombre de sets, difficulté de l'IA). Pilotés au pad par le joueur 1
    /// depuis le <see cref="PlayerSelectionHandler"/> (focus, changement de valeur).
    /// La ligne de difficulté n'est proposée que lorsque le joueur 2 est une IA.
    /// </summary>
    public class MatchSettingsHandler : MonoBehaviour
    {
        [Header("===== SLIDERS =====")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Slider _setCountSlider;

        [Header("===== AI DIFFICULTY =====")]
        [Tooltip("Slider de difficulté : bornes et pas sont réglés automatiquement selon les assets trouvés.")]
        [SerializeField] private Slider _difficultySlider;
        [Tooltip("Texte affichant le nom de la difficulté choisie (optionnel).")]
        [SerializeField] private TextMeshProUGUI _difficultyLabel;
        [Tooltip("Objet masqué quand le joueur 2 est humain (par défaut : le slider lui-même).")]
        [SerializeField] private GameObject _difficultyRow;
        [Tooltip("Dossier Resources contenant les BotDifficulty.")]
        [SerializeField] private string _difficultiesResourcesPath = "BotDifficulty";
        [Tooltip("Difficulté proposée par défaut (position dans la liste triée par Order).")]
        [SerializeField] private int _defaultDifficultyIndex = 1;

        private readonly List<Slider> _focusOrder = new();
        private List<BotDifficulty> _difficulties = new();
        private bool _isDifficultyVisible = true;
        private int _focusIndex = -1;

        public bool IsFocused => _focusIndex >= 0;

        private bool HasDifficulty => _difficultySlider && _difficulties.Count > 0;

        private void Awake()
        {
            if (_difficultySlider)
            {
                _difficulties = ResourceLoader.GetAllScriptables<BotDifficulty>(_difficultiesResourcesPath);
                _difficulties.Sort((a, b) => a.Order.CompareTo(b.Order));
            }

            if (!_difficultyRow && _difficultySlider) _difficultyRow = _difficultySlider.gameObject;

            RebuildFocusOrder();
        }

        private void Start()
        {
            _timerSlider.onValueChanged.AddListener(OnTimerChanged);
            _setCountSlider.onValueChanged.AddListener(OnSetCountChanged);

            _timerSlider.value = GameManager.Instance.SetDuration;
            _setCountSlider.value = GameManager.Instance.SetCountToWinAMatch;

            SetupDifficultySlider();
        }

        #region ===== FOCUS =====

        public void Focus()
        {
            _focusIndex = 0;
            SelectFocused();
        }

        public void Unfocus()
        {
            _focusIndex = -1;
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>Déplace le focus. Renvoie false si on sort par le haut (retour à la grille).</summary>
        public bool MoveFocus(int delta)
        {
            int next = _focusIndex + delta;
            if (next < 0) return false;

            _focusIndex = Mathf.Min(next, _focusOrder.Count - 1);
            SelectFocused();
            return true;
        }

        public void ChangeFocusedValue(int direction)
        {
            if (!IsFocused) return;

            Slider slider = _focusOrder[_focusIndex];
            float step = slider.wholeNumbers ? 1f : (slider.maxValue - slider.minValue) / 10f;
            slider.value = Mathf.Clamp(slider.value + direction * step, slider.minValue, slider.maxValue);
        }

        private void SelectFocused()
        {
            if (EventSystem.current) EventSystem.current.SetSelectedGameObject(_focusOrder[_focusIndex].gameObject);
        }

        private void RebuildFocusOrder()
        {
            _focusOrder.Clear();
            _focusOrder.Add(_timerSlider);
            _focusOrder.Add(_setCountSlider);

            if (HasDifficulty && _isDifficultyVisible) _focusOrder.Add(_difficultySlider);

            if (_focusIndex >= _focusOrder.Count)
            {
                _focusIndex = _focusOrder.Count - 1;
                SelectFocused();
            }
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

        private void SetupDifficultySlider()
        {
            if (!HasDifficulty)
            {
                if (_difficultyRow) _difficultyRow.SetActive(false);
                return;
            }

            _difficultySlider.wholeNumbers = true;
            _difficultySlider.minValue = 0;
            _difficultySlider.maxValue = _difficulties.Count - 1;
            _difficultySlider.onValueChanged.AddListener(OnDifficultyChanged);

            // Garde le choix précédent (retour au menu, revanche), sinon la difficulté par défaut
            int index = _difficulties.IndexOf(GameManager.Instance.BotDifficulty);
            if (index < 0) index = Mathf.Clamp(_defaultDifficultyIndex, 0, _difficulties.Count - 1);

            _difficultySlider.SetValueWithoutNotify(index);
            OnDifficultyChanged(index);

            if (_difficultyRow) _difficultyRow.SetActive(_isDifficultyVisible);
        }

        private void OnDifficultyChanged(float value)
        {
            BotDifficulty difficulty = _difficulties[Mathf.Clamp((int) value, 0, _difficulties.Count - 1)];

            GameManager.Instance.BotDifficulty = difficulty;
            RefreshDifficultyLabel();
        }

        private void RefreshDifficultyLabel()
        {
            if (!_difficultyLabel || !HasDifficulty) return;

            BotDifficulty difficulty = _difficulties[Mathf.Clamp((int) _difficultySlider.value, 0, _difficulties.Count - 1)];
            _difficultyLabel.text = difficulty.GetDisplayName();
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged(Locale locale) => RefreshDifficultyLabel();

        #endregion

        private void OnTimerChanged(float value)
        {
            GameManager.Instance.SetDuration = (int) value;
        }

        private void OnSetCountChanged(float value)
        {
            GameManager.Instance.SetCountToWinAMatch = (int) value;
        }
    }
}
