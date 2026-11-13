using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionPlayer
{
    /// <summary>
    /// Réglage de match affiché sous forme de nombre (remplace un slider).
    /// Trois états visuels : normal, survolé (le curseur est dessus) et en édition (la valeur change au haut / bas).
    /// </summary>
    public class MatchSettingField : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private TextMeshProUGUI _valueText;
        [Tooltip("Fond / cadre teinté selon l'état (optionnel).")]
        [SerializeField] private Graphic _background;
        [Tooltip("Objet affiché pendant l'édition, ex. flèches haut / bas (optionnel).")]
        [SerializeField] private GameObject _editIndicator;

        [Header("===== VALUES =====")]
        [SerializeField] private int _min = 1;
        [SerializeField] private int _max = 5;
        [SerializeField] private int _step = 1;
        [Tooltip("Passe du max au min (et inversement) au lieu de bloquer.")]
        [SerializeField] private bool _wrap;

        [Header("===== COLORS =====")]
        [SerializeField] private Color _normalColor = new(1f, 1f, 1f, 0.15f);
        [SerializeField] private Color _hoveredColor = new(1f, 1f, 1f, 0.45f);
        [SerializeField] private Color _editingColor = new(1f, 0.8f, 0.1f, 0.75f);
        [SerializeField] private Color _valueNormalColor = Color.white;
        [SerializeField] private Color _valueEditingColor = new(1f, 0.85f, 0.2f, 1f);

        [Header("===== FEEDBACK =====")]
        [SerializeField] private float _hoveredScale = 1.08f;
        [SerializeField] private float _tweenDuration = 0.12f;

        private Func<int, string> _formatter;
        private bool _isHovered;
        private bool _isEditing;

        public event Action<int> OnValueChanged;

        public int Value { get; private set; }
        public int Min => _min;
        public int Max => _max;
        public bool IsEditing => _isEditing;

        private void Awake()
        {
            if (_editIndicator) _editIndicator.SetActive(false);
            ApplyVisualState(instant: true);
        }

        private void OnDestroy()
        {
            transform.DOKill();
            if (_valueText) _valueText.transform.DOKill();
        }

        /// <summary>Définit les bornes (ex. nombre de difficultés trouvées).</summary>
        public void SetRange(int min, int max)
        {
            _min = min;
            _max = Mathf.Max(min, max);
            SetValueWithoutNotify(Value);
        }

        /// <summary>Texte affiché pour une valeur (par défaut : le nombre lui-même).</summary>
        public void SetFormatter(Func<int, string> formatter)
        {
            _formatter = formatter;
            RefreshText();
        }

        public void SetValueWithoutNotify(int value)
        {
            Value = Mathf.Clamp(value, _min, _max);
            RefreshText();
        }

        /// <summary>Change la valeur d'un pas dans la direction donnée. Renvoie false si elle n'a pas bougé.</summary>
        public bool Step(int direction)
        {
            if (direction == 0) return false;

            // Se cale sur la grille min + k * pas (ex. 45 avec un pas de 10 → 50 ou 40)
            int step = Mathf.Max(1, _step);
            int offset = Value - _min;
            int stepIndex = direction > 0 ? offset / step + 1 : (offset + step - 1) / step - 1;
            int next = _min + stepIndex * step;

            if (_wrap)
            {
                if (next > _max) next = _min;
                else if (next < _min) next = _max;
            }
            else
            {
                next = Mathf.Clamp(next, _min, _max);
            }

            if (next == Value) return false;

            Value = next;
            RefreshText();
            OnValueChanged?.Invoke(Value);

            if (_valueText)
            {
                _valueText.transform.DOKill(complete: true);
                _valueText.transform.DOPunchPosition(Vector3.up * (8f * direction), _tweenDuration * 1.5f, 6, 0.5f);
            }

            return true;
        }

        public void RefreshText()
        {
            if (_valueText) _valueText.text = _formatter != null ? _formatter(Value) : Value.ToString();
        }

        public void SetHovered(bool isHovered)
        {
            if (_isHovered == isHovered) return;

            _isHovered = isHovered;
            ApplyVisualState(instant: false);
        }

        public void SetEditing(bool isEditing)
        {
            if (_isEditing == isEditing) return;

            _isEditing = isEditing;
            if (_editIndicator) _editIndicator.SetActive(isEditing);
            ApplyVisualState(instant: false);
        }

        private void ApplyVisualState(bool instant)
        {
            float duration = instant ? 0f : _tweenDuration;

            Color backgroundColor = _isEditing ? _editingColor : _isHovered ? _hoveredColor : _normalColor;
            if (_background)
            {
                _background.DOKill();
                _background.DOColor(backgroundColor, duration);
            }

            if (_valueText) _valueText.color = _isEditing ? _valueEditingColor : _valueNormalColor;

            float scale = _isHovered || _isEditing ? _hoveredScale : 1f;
            transform.DOKill(complete: true);
            transform.DOScale(scale, duration);
        }
    }
}
