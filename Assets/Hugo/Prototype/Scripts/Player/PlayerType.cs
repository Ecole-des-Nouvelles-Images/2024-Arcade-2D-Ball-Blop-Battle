using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public abstract class PlayerType : ScriptableObject
    {
        [Header("General Player Data")]
        public string PlayerName;
        public Sprite Sprite;
        public float SpeedSpecialSpike;
        public string SpecialSpikeDescription;
        public RuntimeAnimatorController PlayerAnimatorController;

        public abstract void SpecialSpike(GameObject player, GameObject ball, Vector2 direction);
    }
}
