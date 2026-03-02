using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UICountTouchBall : MonoBehaviour
    {
        [Header("===== Settings =====")]
        [SerializeField] private float _duration;
        [Header("Scale")]
        [SerializeField] private float _scaleAnimationDuration;
        [SerializeField] private AnimationCurve _scaleAnimationCurve;
        [Header("Shake")]
        [SerializeField] private float _shakeAnimationDuration;
        [SerializeField] private float _shakeMagnitude;
        [Header("Fade")]
        [SerializeField] private float _fadeAnimationDuration;
        [SerializeField] private AnimationCurve _fadeAnimationCurve;
        [Header("Color")]
        [SerializeField] private Color _color1;
        [SerializeField] private Color _color2;
        [SerializeField] private Color _color3;
        
        
        [Header("===== References =====")]
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        
        private int _countTouch;

        public void Setup(int countTouch)
        {
            _countTouch = countTouch;
            _textMeshProUGUI.text = _countTouch.ToString();
            
            switch (_countTouch)
            {
                case 1:
                    _textMeshProUGUI.color = _color1;
                    break;
                case 2:
                    _textMeshProUGUI.color = _color2;
                    break;
                case 3:
                    _textMeshProUGUI.color = _color3;
                    break;
            }
            
            _textMeshProUGUI.transform.DOScale(1f, _scaleAnimationDuration).SetEase(_scaleAnimationCurve).SetLink(gameObject);
            if (_countTouch == 3) _textMeshProUGUI.transform.DOShakeRotation(_shakeAnimationDuration, _shakeMagnitude).SetLink(gameObject);

            Invoke(nameof(FadeOut), _duration);
        }

        private void FadeOut()
        {
            _textMeshProUGUI.DOFade(0f, _fadeAnimationDuration).SetEase(_fadeAnimationCurve).SetLink(gameObject);
            Destroy(gameObject, _fadeAnimationDuration * 1.2f);
        }
    }
}
