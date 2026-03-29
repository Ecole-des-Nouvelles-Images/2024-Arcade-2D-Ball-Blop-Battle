using DG.Tweening;
using UnityEngine;

namespace Cam
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Shake Settings")]
        [SerializeField] private float _shakeScoreDuration;
        [SerializeField] private float _shakeScoreMagnitude;
        [SerializeField] private float _shakeHitDuration;
        [SerializeField] private float _shakeHitMagnitude;
        [SerializeField] private Transform _cam;

        public void ScoredShake()
        {
            _cam.DOShakePosition( _shakeScoreDuration, _shakeScoreMagnitude);
            Invoke(nameof(ResetTransform), _shakeScoreDuration + 0.1f);
        }

        public void HitShake()
        {
            _cam.DOShakePosition( _shakeHitDuration, _shakeHitMagnitude);
            Invoke(nameof(ResetTransform), _shakeHitDuration + 0.1f);
        }

        private void ResetTransform()
        {
            transform.position = new Vector3(0,0,-10);
        }
    }
}
