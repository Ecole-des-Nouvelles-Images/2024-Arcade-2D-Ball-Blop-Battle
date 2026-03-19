using DG.Tweening;
using Sounds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Branches.Hugo.OldScripts.UI
{
    public class UIButtonFedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
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
            if (_playTweening)
            {
                transform.DOPause();
                transform.DOScale(_animationEndScale, _animationTime).SetEase(_animationCurve).SetUpdate(true);
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
            if (_playTweening) {
                transform.DOPause();
                transform.DOScale(1, _animationTime).SetEase(_animationCurve).SetUpdate(true);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
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