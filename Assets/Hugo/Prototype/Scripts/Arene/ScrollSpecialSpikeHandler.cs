using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class ScrollSpecialSpikeHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed = 1.0f; // Vitesse d'augmentation

        private void Update()
        {
            // Augmente la position sur l'axe Y à chaque frame
            transform.position += Vector3.up * (_speed * Time.deltaTime);
        }
    }
}
