using _Branches.Hugo.OldScripts.Ball;
using DG.Tweening;
using UnityEngine;

namespace Arene
{
    public class BlopEyesFollowBall : MonoBehaviour
    {
        private OldBallHandler _oldBallHandler;
        
        [Header("Settings")]
        [SerializeField] private Vector3 _basePosition;
        [SerializeField] private Vector3 _leftPosition;
        [SerializeField] private Vector3 _rightPosition;
        [SerializeField] private float _animationTime;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private void Update()
        {
            if (GameObject.FindGameObjectWithTag("Ball"))
            {
                _oldBallHandler = GameObject.FindGameObjectWithTag("Ball").GetComponent<OldBallHandler>();
            }

            if (_oldBallHandler)
            {
                if (_oldBallHandler.IsPlayerOneSide)
                {
                    transform.DOMove(_leftPosition, _animationTime).SetEase(_animationCurve);
                }
                else
                {
                    transform.DOMove(_rightPosition, _animationTime).SetEase(_animationCurve);
                }
            }
            else
            {
                transform.DOMove(_basePosition, _animationTime).SetEase(_animationCurve);
            }
        }
    }
}
