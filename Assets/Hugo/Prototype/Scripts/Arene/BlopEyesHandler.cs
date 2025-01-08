using DG.Tweening;
using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class BlopEyesHandler : MonoBehaviour
    {
        private BallHandler _ballHandler;
        
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
                _ballHandler = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallHandler>();
            }

            if (_ballHandler)
            {
                // Debug.Log(_ballHandler.IsPlayerOneSide);
                if (_ballHandler.IsPlayerOneSide)
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
