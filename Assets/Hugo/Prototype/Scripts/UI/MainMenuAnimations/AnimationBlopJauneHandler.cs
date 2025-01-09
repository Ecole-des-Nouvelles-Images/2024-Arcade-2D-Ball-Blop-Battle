using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.UI.MainMenuAnimations
{
    public class AnimationBlopJauneHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _animationTime;
        [SerializeField] private float _animationStartScale;
        [SerializeField] private float _animationEndScale;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private void Start()
        {
            GoUp();
        }

        private void GoUp()
        {
            transform.DOScaleY(_animationEndScale, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(GoDown), _animationTime);
        }
        
        private void GoDown()
        {
            transform.DOScaleY(_animationStartScale, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(GoUp), _animationTime);
        }
    }
}
