using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Hugo.Prototype.Scripts.UI
{
    public class HUDNumberTouchBall : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
        
        [Header("Settings")]
        [SerializeField] private float _textStayDisplay;
        [SerializeField] private float _animationTime;
        [SerializeField] private float _animationEndScale;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Color _foolColor;
        [SerializeField] private float _textFoolStayDisplay;
        [SerializeField] private float _animationFoolEndScale;
        [SerializeField] private float _shakeMagnitude;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            var color = _textMeshProUGUI.color;
            color.a = 1;
            _textMeshProUGUI.color = color;

            if (_textMeshProUGUI.text == "3")
            {
                Debug.Log("Text = 3");
                _textMeshProUGUI.color = _foolColor;
                transform.DOScale(_animationFoolEndScale, _animationTime).SetEase(_animationCurve);
                transform.DOShakeRotation(_textFoolStayDisplay, _shakeMagnitude);
                Invoke(nameof(FadeOut), _textFoolStayDisplay);
            }
            else
            {
                Debug.Log("Text != 3");
                transform.DOScale(_animationEndScale, _animationTime).SetEase(_animationCurve);
                Invoke(nameof(FadeOut), _textStayDisplay);
            }
        }

        private void FadeOut()
        {
            _textMeshProUGUI.DOFade(0, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(Destroy), _animationTime);
        }

        private void Destroy()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
