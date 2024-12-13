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

        public void ScoredShake()
        {
            _cam.DOShakePosition( _shakeScoreDuration, _shakeScoreMagnitude);
            // Invoke(nameof(ResetTransform), _shakeScoreDuration);
        }

        public void HitShake()
        {
            _cam.DOShakePosition( _shakeHitDuration, _shakeHitMagnitude);
            // Invoke(nameof(ResetTransform), _shakeHitDuration);
        }

        // private void ResetTransform()
        // {
        //     transform.position = Vector2.zero;
        // }
    }
}
