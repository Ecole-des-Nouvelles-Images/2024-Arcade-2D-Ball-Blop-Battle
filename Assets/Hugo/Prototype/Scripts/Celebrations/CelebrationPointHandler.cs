using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Celebrations
{
    public class CelebrationPointHandler : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _startPosition;
        private float _startScale;
        
        [Header("Settings")]
        [SerializeField] private float _animationAppearTime;
        [SerializeField] private float _animationDisappearTime;
        [SerializeField] private float _animationEndPosition;
        [SerializeField] private float _animationEndScale;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _timeBeforeDisappear;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _startPosition = transform.position.x;
            _startScale = transform.localScale.x;
        }

        private void Start()
        {
            if (Mathf.Approximately(transform.position.x, 8))
            {
                transform.DOMoveX(transform.position.x - _animationEndPosition, _animationAppearTime).SetEase(_animationCurve);
                transform.DOScaleX(_animationEndScale, _animationAppearTime).SetEase(_animationCurve);
            }
            else if (Mathf.Approximately(transform.position.x, -8))
            {
                _spriteRenderer.flipX = true;
                
                transform.DOMoveX(transform.position.x + _animationEndPosition, _animationAppearTime).SetEase(_animationCurve);
                transform.DOScaleX(_animationEndScale, _animationAppearTime).SetEase(_animationCurve);
            }
            Invoke(nameof(Disappear), _timeBeforeDisappear);
        }

        private void Disappear()
        {
            transform.DOMoveX(_startPosition, _animationDisappearTime).SetEase(_animationCurve);
            transform.DOScaleX(_startScale, _animationDisappearTime).SetEase(_animationCurve);
            Destroy(gameObject, _animationDisappearTime);
        }
    }
}
