using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.Prototype.Scripts.Arene
{
    public class VentilationHandler : MonoBehaviour
    {
        [FormerlySerializedAs("maxRotationSpeed")]
        [Header("Rotation Settings")]
        [SerializeField] private float _maxRotationSpeed = 360f; // Vitesse maximale en degrés par seconde
        [SerializeField] private float _accelerationDuration = 10f; // Durée pour atteindre la vitesse maximale en secondes

        private float _currentRotationSpeed = 0f; // Vitesse de rotation actuelle
        private float _accelerationRate; // Taux d'accélération calculé

        private void Start()
        {
            // Calculer le taux d'accélération pour atteindre la vitesse maximale
            _accelerationRate = _maxRotationSpeed / _accelerationDuration;
        }

        private void Update()
        {
            // Si la vitesse actuelle est inférieure à la vitesse maximale, accélérer
            if (_currentRotationSpeed < _maxRotationSpeed)
            {
                _currentRotationSpeed += _accelerationRate * Time.deltaTime;

                // Clamp pour éviter de dépasser la vitesse maximale
                if (_currentRotationSpeed > _maxRotationSpeed)
                {
                    _currentRotationSpeed = _maxRotationSpeed;
                }
            }

            // Appliquer la rotation
            transform.Rotate(0f, 0f, _currentRotationSpeed * Time.deltaTime);
        }
    }
}
