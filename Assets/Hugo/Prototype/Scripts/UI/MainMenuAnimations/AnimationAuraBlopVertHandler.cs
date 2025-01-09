using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI.MainMenuAnimations
{
    public class AnimationAuraBlopVertHandler : MonoBehaviour
    {
        private Image _image;
        
        [Header("Settings")]
        [SerializeField] private float _animationTime;
        [SerializeField] private float _animationStartFade;
        [SerializeField] private float _animationEndFade;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            FadeIn();
        }

        private void FadeIn()
        {
            _image.DOFade(_animationStartFade, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(FadeOut), _animationTime);
        }
        
        private void FadeOut()
        {
            _image.DOFade(_animationEndFade, _animationTime).SetEase(_animationCurve);
            Invoke(nameof(FadeIn), _animationTime);
        }
    }
}
