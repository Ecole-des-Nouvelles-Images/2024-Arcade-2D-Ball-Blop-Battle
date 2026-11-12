using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SelectionPlayer
{
    /// <summary>
    /// Réglages du match (durée d'un set, nombre de sets). Pilotés au pad par le joueur 1
    /// depuis le <see cref="PlayerSelectionHandler"/> (focus, changement de valeur).
    /// </summary>
    public class MatchSettingsHandler : MonoBehaviour
    {
        [Header("===== SLIDERS =====")]
        [SerializeField] private Slider _timerSlider;
        [SerializeField] private Slider _setCountSlider;

        private Slider[] _focusOrder;
        private int _focusIndex = -1;

        public bool IsFocused => _focusIndex >= 0;

        private void Awake()
        {
            _focusOrder = new[] { _timerSlider, _setCountSlider };
        }

        private void Start()
        {
            _timerSlider.onValueChanged.AddListener(OnTimerChanged);
            _setCountSlider.onValueChanged.AddListener(OnSetCountChanged);

            _timerSlider.value = GameManager.Instance.SetDuration;
            _setCountSlider.value = GameManager.Instance.SetCountToWinAMatch;
        }

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

            _focusIndex = Mathf.Min(next, _focusOrder.Length - 1);
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
