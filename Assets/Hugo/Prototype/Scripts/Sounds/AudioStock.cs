using System.Collections.Generic;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Sounds
{
    public class AudioStock : MonoBehaviourSingleton<AudioStock>
    {
        [Header("Ball")]
        public List<AudioClip> BallClips;
        
        [Header("Laser")]
        public List<AudioClip> LaserClips;
        
        [Header("UI")]
        public AudioClip SlimeButtonClip;
        public AudioClip ClassicButtonClip;
    }
}
