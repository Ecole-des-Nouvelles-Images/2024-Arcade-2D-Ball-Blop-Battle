using System;
using DG.Tweening;
using Hugo.Prototype.Scripts.Sounds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIButtonFedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
        // [SerializeField] private bool _playSounds = true;
        // [SerializeField] private AudioElement _acPointerEntrer;
        // [SerializeField] private AudioElement _acPointerExit;
        // [SerializeField] private AudioElement _acPointerDown;
        
        private AudioSource _audioSource;

        [SerializeField] private bool _playTweening = true;
        [SerializeField] private float _animationTime = 0.3f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _animationEndScale = 1.2f;
        [SerializeField] private bool _isSlimeButton;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // if (_playSounds) {
            //     AudioManager.Instance?.PlaySFX(_acPointerEntrer);
            // }

            if (_playTweening)
            {
                transform.DOPause();
                transform.DOScale(_animationEndScale, _animationTime).SetEase(_animationCurve);
            }
            
            // SFX
            if (_isSlimeButton)
            {
                _audioSource.clip = AudioStock.Instance.SlimeButtonClip;
                _audioSource.Play();
            }
            else
            {
                _audioSource.clip = AudioStock.Instance.ClassicButtonClip;
                _audioSource.Play();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // if (_playSounds) {
            //     AudioManager.Instance?.PlaySFX(_acPointerExit);
            // }
            
            if (_playTweening) {
                transform.DOPause();
                transform.DOScale(1, _animationTime).SetEase(_animationCurve);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // if (_playSounds) {
            //     AudioManager.Instance?.PlaySFX(_acPointerDown);
            // }
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnPointerEnter(null);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            OnPointerExit(null);
        }
    }
}