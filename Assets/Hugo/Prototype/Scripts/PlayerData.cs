using UnityEngine;

namespace Hugo.Prototype.Scripts
{
    public abstract class PlayerData : ScriptableObject
    {
        public Sprite Sprite;

        public abstract void SpecialSpike();
    }
}
