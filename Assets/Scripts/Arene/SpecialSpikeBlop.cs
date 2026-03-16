using DG.Tweening;
using Player.ScriptableObjects;
using UnityEngine;

namespace Arene
{
    public class SpecialSpikeBlop : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speedDicrase;
        [SerializeField] private AnimationCurve _animationCurve;
        
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
                vector3.x = -5f;
                _spBlop.transform.localPosition = vector3;
                
                transform.DOMoveX(transform.position.x - 10f, _speedDicrase).SetEase(_animationCurve);
            }
            else if (playerId == 2)
            {
                _spBlop.flipX = true;
                var vector3 = _spBlop.transform.localPosition;
                vector3.x = 5f;
                _spBlop.transform.localPosition = vector3;
                
                transform.DOMoveX(transform.position.x + 10f, _speedDicrase).SetEase(_animationCurve);
            }
        }

        private void Update()
        {
            if (!Mathf.Approximately(_spBlop.color.a, 0))
            {
                var colorFront = _spBlop.color;
                colorFront.a -= _speedDicrase * Time.deltaTime;
                _spBlop.color = colorFront;
            }
        }
    }
}