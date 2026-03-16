using Player.ScriptableObjects;
using UnityEngine;

namespace Arene
{
    public class SpecialSpikeImpact : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speedDicrase;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer _spFront;
        [SerializeField] private SpriteRenderer _srImpact;
        [SerializeField] private SpriteRenderer _spBackground;
        
        private Color _color;

        public void Setup(int playerId, BlopType blopType)
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
            
            _spBackground.color = _color;
        }

        private void Update()
        {
            if (!Mathf.Approximately(_spFront.color.a, 0))
            {
                var colorFront = _spFront.color;
                colorFront.a -= _speedDicrase * Time.deltaTime;
                _spFront.color = colorFront;
            }
        }
    }
}