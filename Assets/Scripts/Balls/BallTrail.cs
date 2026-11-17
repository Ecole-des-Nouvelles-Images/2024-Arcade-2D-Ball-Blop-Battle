using Player;
using Player.ScriptableObjects;
using UnityEngine;
using Utils;

namespace Balls
{
    /// <summary>
    /// Colore la traînée de la balle avec la couleur du dernier blop qui l'a touchée.
    /// S'appuie sur <see cref="EventBus.OnPlayerTouchedBall"/> (levé par le PlayerController à chaque contact)
    /// plutôt que sur une détection par zone, qui ratait les frappes rapides et les contacts au filet.
    /// </summary>
    public class BallTrail : MonoBehaviour
    {
        private TrailRenderer _trailRenderer;
        private BlopType? _currentType;
        private PlayerController[] _players;

        private readonly Color _colorBlue = new(0.25f, 0.57f, 0.75f);
        private readonly Color _colorYellow = new(1f, 1f, 0f);
        private readonly Color _colorGreen = new(0.4f, 0.75f, 0.25f);
        private readonly Color _colorRed = new(0.88f, 0.3f, 0.23f);

        private void Awake()
        {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            EventBus.OnPlayerTouchedBall += OnPlayerTouchedBall;
        }

        private void OnDisable()
        {
            EventBus.OnPlayerTouchedBall -= OnPlayerTouchedBall;
        }

        private void OnPlayerTouchedBall(int playerId)
        {
            PlayerController player = FindPlayer(playerId);
            if (!player) return;

            BlopType newType = player.BlopType;
            if (_currentType == newType) return;

            _currentType = newType;
            UpdateTrailColor(newType);
        }

        private PlayerController FindPlayer(int playerId)
        {
            for (int pass = 0; pass < 2; pass++)
            {
                if (_players != null)
                {
                    foreach (PlayerController player in _players)
                    {
                        if (player && player.PlayerId == playerId) return player;
                    }
                }

                // Cache absent ou périmé (joueur détruit / respawn) : on relance une recherche une seule fois
                _players = FindObjectsByType<PlayerController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            }
            return null;
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
