using UnityEngine;

namespace Balls
{
    public interface IBall
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        
        void PerfectReception();
        void DashReception();
        void Absorb(Transform playerTransform);
        void Drawn(Vector2 direction);
        void DrawnSpecialSpike(Vector2 direction, float speed);
    }
}