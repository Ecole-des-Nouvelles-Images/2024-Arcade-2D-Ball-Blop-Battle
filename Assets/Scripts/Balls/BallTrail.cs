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
        
        // On augmente la taille à 4 pour être sûr de capter le joueur 
        // même s'il y a d'autres colliders (trigger, etc.) autour.
        private readonly Collider2D[] _results = new Collider2D[4];

        private readonly Color _colorBlue = new(0.25f, 0.57f, 0.75f);
        private readonly Color _colorYellow = new(1f, 1f, 0f);
        private readonly Color _colorGreen = new(0.4f, 0.75f, 0.25f);
        private readonly Color _colorRed = new(0.88f, 0.3f, 0.23f);

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void FixedUpdate()
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _results, _playerLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_results[i].TryGetComponent(out PlayerController player))
                {
                    BlopType newType = player.BlopType;

                    if (_currentType != newType)
                    {
                        UpdateTrailColor(newType);
                        _currentType = newType;
                    }
                    break; 
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }
    }
}