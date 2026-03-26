using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.AnimationFedback
{
    [RequireComponent(typeof(Image))]
    public class UISpriteAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameRate = 0.1f;
        [SerializeField] private bool _loop = true;

        private Image _imageComponent;
        private int _currentFrame;
        private float _timer;
        private bool _isPlaying = true;

        private void Awake()
        {
            _imageComponent = GetComponent<Image>();
        }

        private void Update()
        {
            if (!_isPlaying || _sprites == null || _sprites.Length == 0) return;

            _timer += Time.deltaTime;

            if (_timer >= _frameRate)
            {
                _timer -= _frameRate;
                _currentFrame++;

                if (_currentFrame >= _sprites.Length)
                {
                    if (_loop)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        _currentFrame = _sprites.Length - 1;
                        _isPlaying = false;
                    }
                }

                _imageComponent.sprite = _sprites[_currentFrame];
            }
        }

        // Fonctions pour contrôler l'animation depuis d'autres scripts
        public void Play() { _isPlaying = true; _currentFrame = 0; }
        public void Stop() { _isPlaying = false; }
    }
}