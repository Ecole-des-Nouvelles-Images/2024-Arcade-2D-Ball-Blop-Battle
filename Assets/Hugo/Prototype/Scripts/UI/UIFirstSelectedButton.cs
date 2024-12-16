using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UIFirstSelectedButton : MonoBehaviour
    {
        [SerializeField] private bool _playTweening = true;
        [SerializeField] private float _animationTime = 0.3f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float _animationEndScale = 1.2f;
        
        [Header("First Selected Button")]
        [SerializeField] private GameObject _firstSelectedButton;
        
        [Header("Buttons")]
        [SerializeField] private List<GameObject> _buttons;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectedButton);
            if (_playTweening)
            {
                _firstSelectedButton.transform.DOPause();
                _firstSelectedButton.transform.DOScale(_animationEndScale, _animationTime).SetEase(_animationCurve);
            }

            foreach (GameObject button in _buttons)
            {
                button.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }
}
