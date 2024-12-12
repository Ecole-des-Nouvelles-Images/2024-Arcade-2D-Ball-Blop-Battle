using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class SignsHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _isFoul;
        
        [Header("Particle System")]
        [SerializeField] private ParticleSystem _particleSystem1;
        [SerializeField] private ParticleSystem _particleSystem2;
        [SerializeField] private ParticleSystem _particleSystem3;
        [SerializeField] private ParticleSystem _particleSystem4;
        
        private void OnEnable()
        {
            Invoke(nameof(SetActiveFalse), 1f);

            if (!_isFoul)
            {
                _particleSystem1.Play();
                _particleSystem2.Play();
                _particleSystem3.Play();
                _particleSystem4.Play();
            }
        }

        private void SetActiveFalse()
        {
            gameObject.SetActive(false);
        }
    }
}
