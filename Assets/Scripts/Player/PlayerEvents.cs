using System;
using Player.ScriptableObjects;
using UnityEngine;

namespace Player
{
    public class PlayerEvents
    {
        // Events
        public event Action<Blop> OnAppears;
        public event Action<Blop> OnMove;
        public event Action<Blop> OnDash;
        public event Action<Blop> OnJump;
        public event Action<Blop> OnDoubleJump;
        public event Action<Blop> OnLand;
        public event Action<Blop> OnPerfectReception;
        public event Action<Blop> OnPunch;
        public event Action<Blop> OnCanAbsorb;
        public event Action<Blop, Transform> OnAbsorb;
        public event Action<Blop, Transform> OnDrawn;
        public event Action<Blop> OnIsWalled;
        public event Action<Blop> OnWallJump;
        public event Action<Blop> OnActiveSpecialSpike;
        public event Action<Blop> OnShootSpecialSpike;
        public event Action<Blop, Transform> OnAbsorbSpecialSpike;
        
        public void Appears(Blop blop) => OnAppears?.Invoke(blop);
        public void Move(Blop blop) => OnMove?.Invoke(blop);
        public void Dash(Blop blop) => OnDash?.Invoke(blop);
        public void Jump(Blop blop) => OnJump?.Invoke(blop);
        public void DoubleJump(Blop blop) => OnDoubleJump?.Invoke(blop);
        public void Land(Blop blop) => OnLand?.Invoke(blop);
        public void PerfectReception(Blop blop) => OnPerfectReception?.Invoke(blop);
        public void Punch(Blop blop) => OnPunch?.Invoke(blop);
        public void CanAbsorb(Blop blop) => OnCanAbsorb?.Invoke(blop);
        public void Absorb(Blop blop, Transform ballTransform) => OnAbsorb?.Invoke(blop, ballTransform);
        public void Drawn(Blop blop, Transform ballTransform) => OnDrawn?.Invoke(blop, ballTransform);
        public void IsWalled(Blop blop) => OnIsWalled?.Invoke(blop);
        public void WallJump(Blop blop) => OnWallJump?.Invoke(blop);
        public void ActiveSpecialSpike(Blop blop) => OnActiveSpecialSpike?.Invoke(blop);
        public void ShootSpecialSpike(Blop blop) => OnShootSpecialSpike?.Invoke(blop);
        public void AbsorbSpecialSpike(Blop blop, Transform ballTransform) => OnAbsorbSpecialSpike?.Invoke(blop, ballTransform);
    }
}
