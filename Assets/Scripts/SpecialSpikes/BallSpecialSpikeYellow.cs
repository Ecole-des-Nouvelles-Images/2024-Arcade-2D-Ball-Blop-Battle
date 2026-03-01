using DG.Tweening;
using UnityEngine;
using Utils;

namespace SpecialSpikes
{
    public class BallSpecialSpikeYellow :  MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _minAlphaValue = 0f;
        [SerializeField] private float _delayDetection = 0.2f;

        [Header("Animation")]
        [SerializeField] private float _animationDuration = 0.25f;
        [SerializeField] private AnimationCurve _animationCurve;
        
        private SpriteRenderer _spriteRenderer;
        private float _spawnTime;
        
        private void Start()
        {
            _spawnTime = Time.time;
            _spriteRenderer = transform.parent.GetComponentInChildren<SpriteRenderer>();
            _spriteRenderer.DOFade(_minAlphaValue, _animationDuration).SetEase(_animationCurve);
        }

        private void Update()
        {
            if (Time.time < _spawnTime + _delayDetection) return;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    _spriteRenderer.DOFade(1f, 0.1f).SetEase(_animationCurve);
                    Destroy(gameObject, _animationDuration * 1.2f);
                }
            }
        }
        
        #region === EVENTS ===

        private void OnEnable()
        {
            EventBus.OnSpecialSpikeActivated += SpecialSpikeActivated;
        }

        private void SpecialSpikeActivated()
        {
            _spriteRenderer.DOFade(1f, 0.1f).SetEase(_animationCurve);
            Destroy(gameObject, _animationDuration * 1.2f);
        }
        
        private void OnDisable()
        {
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
        }

        #endregion
    }
}