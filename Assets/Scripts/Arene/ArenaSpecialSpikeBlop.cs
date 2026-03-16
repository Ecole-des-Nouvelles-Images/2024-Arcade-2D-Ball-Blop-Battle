using DG.Tweening;
using Player.ScriptableObjects;
using UnityEngine;

namespace Arene
{
    public class ArenaSpecialSpikeBlop : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _durationFadeIn;
        [SerializeField] private float _durationMovement;
        [SerializeField] private AnimationCurve _animationFadeCurve;
        [SerializeField] private AnimationCurve _animationMovementCurve;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer _spBlop;
        
        [Header("Sprites")]
        [SerializeField] private Sprite _blue;
        [SerializeField] private Sprite _yellow;
        [SerializeField] private Sprite _green;
        [SerializeField] private Sprite _red;

        public void Setup(int playerId, BlopType blopType)
        {
            switch (blopType)
            {
                case BlopType.Blue:
                    _spBlop.sprite = _blue;
                    break;
                case BlopType.Yellow:
                    _spBlop.sprite = _yellow;
                    break;
                case BlopType.Green:
                    _spBlop.sprite = _green;
                    break;
                case BlopType.Red:
                    _spBlop.sprite = _red;
                    break;
            }

            if (playerId == 1)
            {
                _spBlop.flipX = false;
                var vector3 = _spBlop.transform.localPosition;
                vector3.x = -10f;
                _spBlop.transform.localPosition = vector3;
                
                _spBlop.transform.DOMoveX(_spBlop.transform.position.x + 5f, _durationMovement)
                    .SetEase(_animationMovementCurve);
            }
            else if (playerId == 2)
            {
                _spBlop.flipX = true;
                var vector3 = _spBlop.transform.localPosition;
                vector3.x = 10f;
                _spBlop.transform.localPosition = vector3;
                
                _spBlop.transform.DOMoveX(_spBlop.transform.position.x - 5f, _durationMovement)
                    .SetEase(_animationMovementCurve);
            }
            
            _spBlop.DOFade(1f, _durationFadeIn).SetEase(_animationFadeCurve);
            
            Invoke(nameof(FadeOut), _durationMovement * 0.9f);
        }

        private void FadeOut()
        {
            _spBlop.DOFade(0f, _durationMovement * 0.1f).SetEase(_animationFadeCurve);
        }
    }
}