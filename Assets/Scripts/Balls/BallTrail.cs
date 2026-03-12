using Player;
using Player.ScriptableObjects;
using UnityEngine;

namespace Balls
{
    public class BallTrail : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _detectionRadius = 0.55f;
        [SerializeField] private LayerMask _playerLayer;

        private TrailRenderer _trailRenderer;
        private BlopType? _currentType;
        private Collider2D[] _results = new Collider2D[1];

        private Color _colorBlue = new(0.25f, 0.57f, 0.75f);
        private Color _colorYellow = new(1f, 1f, 0f);
        private Color _colorGreen = new(0.4f, 0.75f, 0.25f);
        private Color _colorRed = new(0.88f, 0.3f, 0.23f);

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void Update()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _results, _playerLayer);

            if (hitCount > 0)
            {
                if (_results[0].TryGetComponent(out PlayerController player))
                {
                    BlopType newType = player.BlopType;

                    if (_currentType != newType)
                    {
                        UpdateTrailColor(newType);
                        _currentType = newType;
                    }
                }
            }
        }

        private void UpdateTrailColor(BlopType type)
        {
            Color targetColor = type switch
            {
                BlopType.Blue   => _colorBlue,
                BlopType.Yellow => _colorYellow,
                BlopType.Green  => _colorGreen,
                BlopType.Red    => _colorRed,
                _               => Color.white
            };

            _trailRenderer.startColor = targetColor;
            _trailRenderer.endColor = targetColor;
        }
    }
}