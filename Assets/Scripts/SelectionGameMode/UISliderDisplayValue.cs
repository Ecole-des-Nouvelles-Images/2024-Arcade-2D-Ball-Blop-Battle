using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SelectionGameMode
{
    public class UISliderDisplayValue : MonoBehaviour
    {
        [Header("===== REFERENCES =====")]
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
            _textMeshProUGUI.text = _slider.value.ToString();
        }
        
        private void OnValueChanged(float value)
        {
            _textMeshProUGUI.text = value.ToString();
        }
    }
}