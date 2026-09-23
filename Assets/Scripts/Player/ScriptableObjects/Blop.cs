using UnityEngine;

namespace Player.ScriptableObjects
{
    public abstract class Blop : ScriptableObject
    {
        [Header("Blop Data")]
        public BlopType BlopType;
        public string PlayerName;
        public Sprite Sprite;
        public RuntimeAnimatorController PlayerAnimatorController;
        
        [Header("Movement Settings")]
        public float Speed;
        public float JumpForce;
        public float WallJumpForce;
        public float JumpingSpeed;
        public float AirControlFactor;
        public float MaxAirSpeed;
        public float DownwardsForce = 1f;
        
        [Header("To Named")]
        public float TimeAppears;
        public float TimeResetRotation;
        
        [Header("Dash Settings")]
        public float DashSpeed;
        public float DashDuration;
        public float DashCooldown;
        
        [Header("Perfect Reception Settings")]
        public float PerfectReceptionDuration;
        public float PerfectReceptionCooldown;
        
        [Header("Is Grounded")]
        public float RayGroundedLength;
        public float RayGroundedLengthHaveTheBall;
        public LayerMask GroundLayer;
        
        [Header("Is Walled")]
        public float RayWalledLength;
        public float RayNetTouchedLength;
        public LayerMask WallLayer;
        
        [Header("Special Spike")]
        public float SpeedSpecialSpike;
        public string SpecialSpikeDescription;
        public GameObject PlayerSpecialSpike;
        public GameObject BallSpecialSpike;
        
        [Header("Particles Systems")]
        public GameObject PSTrailRenderer;
        public GameObject PSAppears;
        public GameObject PSDeath;
        public GameObject PSJump;
        public GameObject PSChocWave;
        public GameObject PSSpitOut;
        public GameObject PSPerfectReception;
        public GameObject PSLanding;
        public GameObject PSActiveSpecialSpike;
        public GameObject PSShootSpecialSpike;


        public abstract void SpecialSpike(GameObject player, GameObject ball, Vector2 direction);
    }
}
