using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.UI.MainMenuAnimations
{
    public class AnimationLogoTextHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _animationTime;
        [SerializeField] private Vector3 _animationLeftRotation;
        [SerializeField] private Vector3 _animationRightRotation;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private void Start()
        {
            GoLeft();
        }

        private void GoLeft()
        {
            transform.DORotate(_animationLeftRotation, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(GoRight), _animationTime);
        }
        
        private void GoRight()
        {
            transform.DORotate(_animationRightRotation, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(GoLeft), _animationTime);
        }
    }
}
