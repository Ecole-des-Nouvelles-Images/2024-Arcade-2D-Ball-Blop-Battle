using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Celebrations
{
    public class CelebrationSetHandler : MonoBehaviour
    {
        // game object component
        private SpriteRenderer _spriteRenderer;
        
        [Header("Settings")]
        [SerializeField] private float _animationAppearTime;
        [SerializeField] private float _animationDisappearTime;
        [SerializeField] private float _animationStartPosition;
        [SerializeField] private float _animationStartScale;
        [SerializeField] private float _animationEndPosition;
        [SerializeField] private float _animationVictoryEndPosition;
        [SerializeField] private float _animationEndScale;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _timeBeforeDisappear;
        
        [Header("Sprites")]
        [SerializeField] private Sprite _firstSprite;
        [SerializeField] private Sprite _secondSprite;
        [SerializeField] private float _interval;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(Coroutine());
        }

        public void SetUp(bool playerOneWinSet, float score)
        {
            if (playerOneWinSet == false)
            {
                _spriteRenderer.flipX = true;
            }

            if (Mathf.Approximately(score, 3))
            {
                _spriteRenderer.sortingOrder = -7;
                transform.DOMoveY(_animationVictoryEndPosition, _animationAppearTime).SetEase(_animationCurve);
                transform.DOScaleY(_animationEndScale, _animationAppearTime).SetEase(_animationCurve);
                Invoke(nameof(Disappear), _timeBeforeDisappear);
            }
            else
            {
                transform.DOMoveY(transform.position.y + _animationEndPosition, _animationAppearTime).SetEase(_animationCurve);
                transform.DOScaleY(_animationEndScale, _animationAppearTime).SetEase(_animationCurve);
                Invoke(nameof(Disappear), _timeBeforeDisappear);
            }
        }

        private void Disappear()
        {
            transform.DOMoveY(_animationStartPosition, _animationDisappearTime).SetEase(_animationCurve);
            transform.DOScaleY(_animationStartScale, _animationDisappearTime).SetEase(_animationCurve);
            Destroy(gameObject, _animationDisappearTime);
        } 
        
        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (_spriteRenderer.sprite == _firstSprite)
                {
                    _spriteRenderer.sprite = _secondSprite;
                }
                else
                {
                    _spriteRenderer.sprite = _firstSprite;
                }
                yield return new WaitForSeconds(_interval);
            }
        }
    }
}
