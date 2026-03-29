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
        

        private void Update()
        {
            if (MatchManager.Instance.BallSide == 1)
            {
                transform.DOMove(_leftPosition, _animationTime).SetEase(_animationCurve);
            }
            else if (MatchManager.Instance.BallSide == 2)
            {
                transform.DOMove(_rightPosition, _animationTime).SetEase(_animationCurve);
            }
            else if (MatchManager.Instance.BallSide == 0)
            {
                transform.DOMove(_basePosition, _animationTime).SetEase(_animationCurve);
            }
        }
    }
}
