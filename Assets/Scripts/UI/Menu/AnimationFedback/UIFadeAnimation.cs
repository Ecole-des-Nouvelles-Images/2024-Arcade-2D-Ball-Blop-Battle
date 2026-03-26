using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimationFedback
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFadeAnimation : UIAnimationBase
    {
        [Header("Fade Settings")]
        [SerializeField] private float _startAlpha = 0f;
        [SerializeField] private float _endAlpha = 1f;

        private CanvasGroup _canvasGroup;

        private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

        public override void Play()
        {
            _currentTween?.Kill();
            _canvasGroup.alpha = _startAlpha;
            
            _currentTween = _canvasGroup.DOFade(_endAlpha, _duration)
                .SetDelay(_delay)
                .SetEase(_animationCurve);

            if (_isLooping) _currentTween.SetLoops(-1, _loopType);
        }
    }
}