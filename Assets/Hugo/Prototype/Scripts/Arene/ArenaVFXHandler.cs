using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class ArenaVFXHandler : MonoBehaviour
    {
        // VFX
        [Header("VFX Effects")]
        [SerializeField] private ParticleSystem _windPlayerOne;
        [SerializeField] private ParticleSystem _windPlayerTwo;

        public void PlayWindPlayerOne()
        {
            _windPlayerOne.Play();
        }
        
        public void PlayWindPlayerTwo()
        {
            _windPlayerTwo.Play();
        }
    }
}
