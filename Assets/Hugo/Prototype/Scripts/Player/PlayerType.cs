using System;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public abstract class PlayerType : ScriptableObject
    {
        [Header("General Player Data")]
        public Sprite Sprite;
        public float SpeedSpecialSpike;
        public String SpecialSpikeName;
        public String SpecialSpikeDescription;
        public RuntimeAnimatorController PlayerAnimatorController;

        public abstract void SpecialSpike(GameObject player, GameObject ball, Vector2 direction);
    }
}
