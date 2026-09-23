using UnityEngine;
using UnityEngine.Serialization;

namespace Arene
{
    public class VentilationHandler : MonoBehaviour
    {
        [FormerlySerializedAs("maxRotationSpeed")]
        [Header("Rotation Settings")]
        [SerializeField] private float _maxRotationSpeed = 360f;
        [SerializeField] private float _accelerationDuration = 10f;

        private float _currentRotationSpeed = 0f;
        private float _accelerationRate;

        private void Start()
        {
            _accelerationRate = _maxRotationSpeed / _accelerationDuration;
        }

        private void Update()
        {
            if (_currentRotationSpeed < _maxRotationSpeed)
            {
                _currentRotationSpeed += _accelerationRate * Time.deltaTime;

                if (_currentRotationSpeed > _maxRotationSpeed)
                {
                    _currentRotationSpeed = _maxRotationSpeed;
                }
            }

            transform.Rotate(0f, 0f, _currentRotationSpeed * Time.deltaTime);
        }
    }
}
