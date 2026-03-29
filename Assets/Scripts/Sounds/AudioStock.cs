using System.Collections.Generic;
using UnityEngine;
using Utils.Singletons;

namespace Sounds
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
