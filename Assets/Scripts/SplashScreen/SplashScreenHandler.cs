using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SplashScreen
{
    public class SplashScreenHandler : MonoBehaviour
    {
        [Header("Timing Settings")]
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _fadeOutDuration = 1f;
        [SerializeField] private float _displayTimePerImage = 2f;
        
        [Header("References")]
        [SerializeField] private CanvasGroup _groupEnsi;
        [SerializeField] private CanvasGroup _groupPackages;
        [SerializeField] private CanvasGroup _groupIllustration;
        [SerializeField] private TextMeshProUGUI _textChargement;

        [Header("Loading Text Settings")]
        [SerializeField] private float _dotInterval = 0.5f;

        private Sequence _mainSequence;

        private void Start()
        {
            SetupInitialState();
            
            BuildAndPlaySequence();
            
            AnimateLoadingText();
        }

        private void SetupInitialState()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _groupEnsi.alpha = 0;
            _groupPackages.alpha = 0;
            _groupIllustration.alpha = 0;
            _textChargement.alpha = 0;
        }

        private void BuildAndPlaySequence()
        {
            _mainSequence = DOTween.Sequence();

            // 1. Apparition ENSI
            _mainSequence.Append(_groupEnsi.DOFade(1f, _fadeInDuration));
            _mainSequence.AppendInterval(_displayTimePerImage);
            _mainSequence.Append(_groupEnsi.DOFade(0f, _fadeOutDuration));

            // 2. Apparition PACKAGES
            _mainSequence.Append(_groupPackages.DOFade(1f, _fadeInDuration));
            _mainSequence.AppendInterval(_displayTimePerImage);
            _mainSequence.Append(_groupPackages.DOFade(0f, _fadeOutDuration));

            // 3. Apparition ILLUSTRATION + ANIMATION + TEXTE
            // On utilise "Join" pour qu'ils apparaissent en même temps
            _mainSequence.Append(_groupIllustration.DOFade(1f, _fadeInDuration));
            _mainSequence.Join(_textChargement.DOFade(1f, _fadeInDuration));
            
            _mainSequence.AppendInterval(_displayTimePerImage);

            // 4. Fin et changement de scène
            _mainSequence.OnComplete(() => SceneManager.LoadScene(1));
        }

        private void AnimateLoadingText()
        {
            string baseText = "Chargement";
            int dots = 0;

            // On crée un intervalle de X secondes, qui boucle à l'infini (-1)
            DOTween.Sequence()
                .AppendInterval(_dotInterval)
                .OnStepComplete(() => {
                    dots = (dots + 1) % 4;
                    _textChargement.text = baseText + new string('.', dots);
                })
                .SetLoops(-1); 
        }

        private void OnDestroy()
        {
            _mainSequence?.Kill();
        }
    }
}