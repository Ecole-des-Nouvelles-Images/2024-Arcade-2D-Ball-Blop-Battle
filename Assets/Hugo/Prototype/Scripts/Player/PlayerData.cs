using System;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public abstract class PlayerData : ScriptableObject
    {
        public Sprite Sprite;
        public String SpecialSpikeName;
        public String SpecialSpikeDescription;

        public abstract void SpecialSpike();
    }
}
