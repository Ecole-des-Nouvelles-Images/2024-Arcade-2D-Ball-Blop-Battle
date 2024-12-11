using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hugo.Prototype.Scripts.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Shake Settings")]
        [SerializeField] private float _shakeDuration; // Durée totale de la secousse
        [SerializeField] private float _shakeMagnitude; // Amplitude de la secousse

        private Vector3 _originalPosition; // Position initiale de la caméra

        private void Awake()
        {
            // Stocker la position de départ
            _originalPosition = transform.localPosition;
        }

        public void StartShake()
        {
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _shakeDuration)
            {
                // Générer un léger décalage aléatoire
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f) * _shakeMagnitude,
                    Random.Range(-1f, 1f) * _shakeMagnitude,
                    0f // On ne change pas l'axe Z pour éviter de désaligner la caméra
                );

                // Appliquer le décalage à la position
                transform.localPosition = _originalPosition + randomOffset;

                // Incrémenter le temps écoulé
                elapsedTime += Time.deltaTime;

                // Attendre la prochaine frame
                yield return null;
            }

            // Réinitialiser la position une fois le shake terminé
            transform.localPosition = _originalPosition;
        }
    }
}
