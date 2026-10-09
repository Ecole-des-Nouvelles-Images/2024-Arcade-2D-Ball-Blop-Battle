using DG.Tweening;
using UnityEngine;

namespace UI.Menu.AnimationFeedback
{
    public class UIMoveVerticalAnimation : UIAnimationBase
    {
        [Header("=== MOVE SETTINGS ===")]
        [SerializeField] private float _startY = -50f;
        [SerializeField] private float _endY = 50f;
        [SerializeField] private float _loopDelay = 0f;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public override void Play()
        {
            if (_rectTransform == null) return;
            
            Kill();

            // Appliquer la position de départ
            Vector2 anchoredPos = _rectTransform.anchoredPosition;
            anchoredPos.y = _startY;
            _rectTransform.anchoredPosition = anchoredPos;

            if (_isLooping && _loopDelay > 0f)
            {
                Sequence seq = DOTween.Sequence();
                
                // 1. Si on a un _delay global, on l'ajoute UNE SEULE FOIS au tout début de la séquence
                if (_delay > 0f)
                {
                    seq.AppendInterval(_delay);
                }

                // 2. Le cycle de l'animation
                seq.Append(_rectTransform.DOAnchorPosY(_endY, _duration).SetEase(_animationCurve));
                seq.Append(_rectTransform.DOAnchorPosY(_startY, _duration).SetEase(_animationCurve));
                seq.AppendInterval(_loopDelay); // La pause entre les boucles

                // 3. On boucle la séquence (le SetLoops va répéter uniquement le cycle et le loopDelay, sans rejouer le _delay initial)
                seq.SetLoops(-1);
                seq.SetUpdate(true);
                seq.SetLink(gameObject);

                _currentTween = seq;
            }
            else
            {
                // Comportement standard (sans loopDelay)
                _currentTween = _rectTransform.DOAnchorPosY(_endY, _duration)
                    .SetDelay(_delay)
                    .SetEase(_animationCurve)
                    .SetUpdate(true)
                    .SetLink(gameObject);

                if (_isLooping) 
                {
                    _currentTween.SetLoops(-1, _loopType);
                }
            }
        }
    }
}