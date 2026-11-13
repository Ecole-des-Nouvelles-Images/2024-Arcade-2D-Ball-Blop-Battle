using UnityEngine;

namespace Bots
{
    public struct BallSample
    {
        public Vector2 Position;
        public Vector2 Velocity;
        /// <summary>Temps relatif à "maintenant" (peut être négatif si la simulation part d'un état perçu en retard).</summary>
        public float Time;
    }

    /// <summary>
    /// Simule la trajectoire de la balle (gravité, murs, plafond, filet) avec le même schéma
    /// d'intégration que Rigidbody2D (Euler semi-implicite au pas de Time.fixedDeltaTime).
    /// Aucune allocation pendant la simulation.
    /// </summary>
    public class BallPredictor
    {
        private readonly BallSample[] _samples;

        public int Count { get; private set; }
        public bool HasLanding { get; private set; }
        public BallSample Landing { get; private set; }
        public bool HitNet { get; private set; }

        public BallPredictor(int capacity = 256)
        {
            _samples = new BallSample[capacity];
        }

        public BallSample this[int index] => _samples[index];

        /// <param name="startTime">Temps du premier état (ex : -reactionTime pour un état perçu en retard).</param>
        public void Simulate(Vector2 position, Vector2 velocity, float gravity, float radius,
            in ArenaBounds arena, float horizon, float dt, float startTime = 0f)
        {
            Count = 0;
            HasLanding = false;
            HitNet = false;

            int steps = Mathf.Min(_samples.Length, Mathf.CeilToInt(horizon / dt));
            float time = startTime;

            for (int i = 0; i < steps; i++)
            {
                Vector2 previous = position;

                velocity.y += gravity * dt;
                position += velocity * dt;
                time += dt;

                // Murs
                if (position.x - radius < arena.LeftWallX)
                {
                    position.x = arena.LeftWallX + radius;
                    velocity.x = Mathf.Abs(velocity.x) * arena.Bounciness;
                }
                else if (position.x + radius > arena.RightWallX)
                {
                    position.x = arena.RightWallX - radius;
                    velocity.x = -Mathf.Abs(velocity.x) * arena.Bounciness;
                }

                // Plafond
                if (position.y + radius > arena.CeilingY)
                {
                    position.y = arena.CeilingY - radius;
                    velocity.y = -Mathf.Abs(velocity.y) * arena.Bounciness;
                }

                // Filet (boîte du sol jusqu'à NetTopY)
                if (position.y - radius < arena.NetTopY
                    && Mathf.Abs(position.x - arena.NetX) < arena.NetHalfWidth + radius)
                {
                    HitNet = true;

                    if (previous.y - radius >= arena.NetTopY)
                    {
                        position.y = arena.NetTopY + radius;
                        velocity.y = Mathf.Abs(velocity.y) * arena.Bounciness;
                    }
                    else
                    {
                        float side = Mathf.Sign(previous.x - arena.NetX);
                        if (side == 0f) side = -Mathf.Sign(velocity.x);
                        position.x = arena.NetX + side * (arena.NetHalfWidth + radius);
                        velocity.x = side * Mathf.Abs(velocity.x) * arena.Bounciness;
                    }
                }

                BallSample sample = new BallSample { Position = position, Velocity = velocity, Time = time };
                _samples[Count++] = sample;

                // Sol : fin de l'échange
                if (position.y - radius <= arena.GroundY)
                {
                    HasLanding = true;
                    Landing = sample;
                    return;
                }
            }
        }

        /// <summary>Premier échantillon futur (Time >= 0).</summary>
        public int FirstFutureIndex()
        {
            for (int i = 0; i < Count; i++)
            {
                if (_samples[i].Time >= 0f) return i;
            }
            return -1;
        }

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            for (int i = 1; i < Count; i++)
            {
                Gizmos.DrawLine(_samples[i - 1].Position, _samples[i].Position);
            }

            if (HasLanding)
            {
                Gizmos.DrawWireSphere(Landing.Position, 0.2f);
            }
        }
    }
}
