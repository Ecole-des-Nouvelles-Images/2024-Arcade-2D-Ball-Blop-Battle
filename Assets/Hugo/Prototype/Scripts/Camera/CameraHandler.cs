using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Shake Settings")]
        [SerializeField] private float _shakeScoreDuration; // Durée totale de la secousse
        [SerializeField] private float _shakeScoreMagnitude; // Amplitude de la secousse
        [SerializeField] private float _shakeHitDuration; // Durée totale de la secousse
        [SerializeField] private float _shakeHitMagnitude; // Amplitude de la secousse
        [SerializeField] private Transform _cam;

        private void Start()
        {
            ScoredShake();
        }

        public void ScoredShake()
        {
            _cam.DOShakePosition( _shakeScoreDuration, _shakeScoreMagnitude);
        }

        public void HitShake()
        {
            _cam.DOShakePosition( _shakeHitDuration, _shakeHitMagnitude);
        }

        // public void StartShake()
        // {
        //     StartCoroutine(ShakeCoroutine());
        // }
        //
        // private IEnumerator ShakeCoroutine()
        // {
        //     float elapsedTime = 0f;
        //
        //     while (elapsedTime < _shakeDuration)
        //     {
        //         // Générer un léger décalage aléatoire
        //         Vector3 randomOffset = new Vector3(
        //             Random.Range(-1f, 1f) * _shakeMagnitude,
        //             Random.Range(-1f, 1f) * _shakeMagnitude,
        //             0f // On ne change pas l'axe Z pour éviter de désaligner la caméra
        //         );
        //
        //         // Appliquer le décalage à la position
        //         transform.localPosition = _originalPosition + randomOffset;
        //
        //         // Incrémenter le temps écoulé
        //         elapsedTime += Time.deltaTime;
        //
        //         // Attendre la prochaine frame
        //         yield return null;
        //     }
        //
        //     // Réinitialiser la position une fois le shake terminé
        //     transform.localPosition = _originalPosition;
        // }
    }
}
