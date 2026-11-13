using Player;
using UnityEngine;

namespace Bots
{
    /// <summary>
    /// Traduit les intentions du bot en inputs pour le PlayerController, comme une vraie manette :
    /// les boutons ne sont envoyés que sur changement d'état (appui / relâchement),
    /// ce qui évite les sauts, dashs et doubles sauts déclenchés à chaque tick.
    /// </summary>
    public class BotActuator
    {
        private readonly PlayerController _player;

        private bool? _south;
        private bool? _west;

        public Vector2 Move { get; private set; }
        public bool IsWestPressed => _west == true;

        public BotActuator(PlayerController player)
        {
            _player = player;
        }

        /// <summary>Le joystick est renvoyé à chaque appel : le PlayerController l'ignore pendant un dash.</summary>
        public void SetMove(Vector2 move)
        {
            Move = move;
            _player.GetJoystickReadValue(move);
        }

        public void SetMoveX(float x) => SetMove(new Vector2(Mathf.Clamp(x, -1f, 1f), 0f));

        public void SetSouth(bool pressed)
        {
            if (_south == pressed) return;
            _south = pressed;
            _player.GetSouthButtonReadValue(pressed ? 1f : 0f);
        }

        public void SetWest(bool pressed)
        {
            if (_west == pressed) return;
            _west = pressed;
            _player.GetWestButtonReadValue(pressed ? 1f : 0f);
        }

        /// <summary>Relâche puis appuie : garantit un front montant (saut, double saut, dash).</summary>
        public void TapSouth()
        {
            SetSouth(false);
            SetSouth(true);
        }

        public void TapWest()
        {
            SetWest(false);
            SetWest(true);
        }

        public void ReleaseAll()
        {
            SetMove(Vector2.zero);
            SetSouth(false);
            SetWest(false);
        }

        /// <summary>À appeler quand le joueur réapparaît : l'état des boutons n'est plus connu.</summary>
        public void Invalidate()
        {
            _south = null;
            _west = null;
        }
    }
}
