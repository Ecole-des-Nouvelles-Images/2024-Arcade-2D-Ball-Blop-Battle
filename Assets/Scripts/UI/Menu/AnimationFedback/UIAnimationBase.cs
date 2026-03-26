using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimationFedback
{
    public abstract class UIAnimationBase : MonoBehaviour
    {
        [Header("Global Settings")]
        [SerializeField] protected float _duration = 0.5f;
        [SerializeField] protected float _delay = 0f;
        [SerializeField] protected AnimationCurve _animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected bool _playOnEnable = true;
        [SerializeField] protected bool _isLooping = false;
        [SerializeField] protected LoopType _loopType = LoopType.Yoyo;

        protected Tween _currentTween;

        protected virtual void OnEnable()
        {
            if (_playOnEnable) Play();
        }

        protected virtual void OnDisable()
        {
            _currentTween?.Kill();
        }

        public abstract void Play();

        public void Kill() => _currentTween?.Kill();
    }
}