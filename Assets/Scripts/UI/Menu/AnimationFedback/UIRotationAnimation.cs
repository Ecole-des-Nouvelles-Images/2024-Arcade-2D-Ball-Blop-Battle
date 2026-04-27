using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimationFedback
{
    public class UIRotationAnimation : UIAnimationBase
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 _startRotation = new Vector3(0, 0, -10f);
        [SerializeField] private Vector3 _endRotation = new Vector3(0, 0, 10f);
        
        [SerializeField] private RotationDirection _direction = RotationDirection.Clockwise;

        public override void Play()
        {
            Kill();
            
            transform.localEulerAngles = _startRotation;
            
            RotateMode mode = (_direction == RotationDirection.Clockwise) 
                ? RotateMode.Fast
                : RotateMode.FastBeyond360;
            
            _currentTween = transform.DOLocalRotate(_endRotation, _duration, mode)
                .SetDelay(_delay)
                .SetEase(_animationCurve)
                .SetUpdate(true);

            if (_isLooping) _currentTween.SetLoops(-1, _loopType);
        }
    }
    
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }
}