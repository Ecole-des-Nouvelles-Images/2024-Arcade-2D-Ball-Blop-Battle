using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimationFedback
{
    public class UIScaleAnimation : UIAnimationBase
    {
        [Header("Scale Settings")]
        [SerializeField] private Vector3 _startScale = Vector3.zero;
        [SerializeField] private Vector3 _endScale = Vector3.one;

        public override void Play()
        {
            _currentTween?.Kill();
            transform.localScale = _startScale;

            _currentTween = transform.DOScale(_endScale, _duration)
                .SetDelay(_delay)
                .SetEase(_animationCurve);

            if (_isLooping) _currentTween.SetLoops(-1, _loopType);
        }
    }
}