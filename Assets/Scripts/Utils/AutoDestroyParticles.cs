using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyParticles : MonoBehaviour
    {
        private void Start()
        {
            var ps = GetComponent<ParticleSystem>();
            var main = ps.main;
        
            main.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}