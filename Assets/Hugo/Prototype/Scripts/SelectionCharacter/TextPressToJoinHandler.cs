using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class TextPressToJoinHandler : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _animationTime;
        [SerializeField] private float _animationStartScale;
        [SerializeField] private float _animationEndScale;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _timeBetweenAnimations;

        private void Start()
        {
            StartCoroutine(ScaleUp(0.1f));
        }

        private IEnumerator ScaleUp(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            transform.DOScale(_animationEndScale, _animationTime).SetEase(_animationCurve);
            StartCoroutine(ScaleDown(_timeBetweenAnimations));
        }

        private IEnumerator ScaleDown(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            transform.DOScale(_animationStartScale, _animationTime).SetEase(_animationCurve);
            StartCoroutine(ScaleUp(_timeBetweenAnimations));
        }
    }
}
