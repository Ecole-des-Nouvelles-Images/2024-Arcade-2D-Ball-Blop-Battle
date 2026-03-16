using System.Collections.Generic;
using Player.ScriptableObjects;
using UnityEngine;

namespace Arene
{
    public class SpecialSpikeScroll : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed;
        [SerializeField] private List<Sprite> _scrollBackGround;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer _srScroll;
        [SerializeField] private ParticleSystem _psScroll;

        private float Timer;
        private int _advancement;
        private Color _color;

        public void Setup(BlopType blopType)
        {
            switch (blopType)
            {
                case BlopType.Blue:
                    _color = new(0.25f, 0.57f, 0.75f);
                    break;
                case BlopType.Yellow:
                    _color = new(1f, 1f, 0f);
                    break;
                case BlopType.Green:
                    _color = new(0.4f, 0.75f, 0.25f);
                    break;
                case BlopType.Red:
                    _color = new(0.88f, 0.3f, 0.23f);
                    break;
            }
            
            _srScroll.color = _color;
            
            var main = _psScroll.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_color, Color.white);
        }

        private void Update()
        {
            Timer += Time.deltaTime;

            if (Timer >= _speed)
            {
                _srScroll.sprite = _scrollBackGround[_advancement];
                
                _advancement++;
                Timer = 0f;

                if (_advancement >= 3)
                {
                    _advancement = 0;
                }
            }
        }
    }
}
