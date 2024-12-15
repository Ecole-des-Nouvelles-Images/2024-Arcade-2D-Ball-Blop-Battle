using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.SlpashScreen
{
    public class SplashScreenHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _screenStay;
        [SerializeField] private float _lauchGame;
        
        [Header("References")]
        [SerializeField] private Image _screenEnsi;
        [SerializeField] private Image _illustration;
        [SerializeField] private TextMeshProUGUI _textChargement;
        [SerializeField] private SpriteRenderer _animationChargement;

        private void Start()
        {
            _screenEnsi.DOFade(1f, _fadeInDuration);
            Invoke(nameof(FadeOutEnsi), _screenStay);
        }

        private void FadeOutEnsi()
        {
            _screenEnsi.DOFade(0f, _fadeOutDuration);
            Invoke(nameof(FadeInLoadingScreen), _fadeOutDuration);
        }

        private void FadeInLoadingScreen()
        {
            _animationChargement.gameObject.SetActive(true);
            _animationChargement.DOFade(1f, _fadeInDuration);
            _illustration.DOFade(1f, _fadeInDuration);
            _textChargement.DOFade(1f, _fadeInDuration);
            Invoke(nameof(ChangeScene), _lauchGame);
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
