using DG.Tweening;
using UnityEngine;
using Utils;

namespace SpecialSpikes
{
    public class BallSpecialSpikeBlue :  MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _minScaleValue = 0.5f;
        [SerializeField] private float _delayDetection = 0.2f;

        [Header("Animation")]
        [SerializeField] private float _animationDuration = 0.25f;
        [SerializeField] private AnimationCurve _animationCurve;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _psSizeRedusction;
        
        private Transform _parentTransform;
        private float _spawnTime;

        private void Awake()
        {
            Instantiate(_psSizeRedusction, transform.position, Quaternion.identity, transform);
        }

        private void Start()
        {
            _spawnTime = Time.time;
            _parentTransform = transform.parent;
            _parentTransform.DOScale(_minScaleValue, _animationDuration).SetEase(_animationCurve);
        }

        private void Update()
        {
            if (Time.time < _spawnTime + _delayDetection) return;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(_parentTransform.position, _minScaleValue);

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    _parentTransform.DOScale(1f, _animationDuration).SetEase(_animationCurve);
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
            _parentTransform.DOScale(1f, _animationDuration).SetEase(_animationCurve);
            Destroy(gameObject, _animationDuration * 1.2f);
        }
        
        private void OnDisable()
        {
            EventBus.OnSpecialSpikeActivated -= SpecialSpikeActivated;
        }

        #endregion
    }
}