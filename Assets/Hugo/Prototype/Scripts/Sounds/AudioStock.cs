using System.Collections.Generic;
using Int.Scripts.Utils.Singletons;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Sounds
{
    public class AudioStock : MonoBehaviourSingletonDontDestroyOnLoad<AudioStock>
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
