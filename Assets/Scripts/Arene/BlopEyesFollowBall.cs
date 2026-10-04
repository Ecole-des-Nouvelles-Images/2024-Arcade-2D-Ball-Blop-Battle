using DG.Tweening;
using Managers;
using UnityEngine;

namespace Arene
{
    public class BlopEyesFollowBall : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _basePosition;
        [SerializeField] private Vector3 _leftPosition;
        [SerializeField] private Vector3 _rightPosition;
        [SerializeField] private float _animationTime;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Tween _moveTween;
        private int _lastBallSide = -1;

        private void Update()
        {
            if (MatchManager.Instance == null) return;
            
            int currentBallSide = MatchManager.Instance.BallSide;
            
            if (currentBallSide != _lastBallSide)
            {
                _moveTween?.Kill();
                
                Vector3 targetPos = currentBallSide switch
                {
                    1 => _leftPosition,
                    2 => _rightPosition,
                    _ => _basePosition
                };
                
                _moveTween = transform.DOMove(targetPos, _animationTime).SetEase(_animationCurve);
                _lastBallSide = currentBallSide;
            }
        }

        private void OnDestroy()
        {
            _moveTween?.Kill();
        }
    }
}
