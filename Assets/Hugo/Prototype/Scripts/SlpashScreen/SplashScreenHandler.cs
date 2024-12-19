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
        [SerializeField] private float _timeScreenEnsiStay;
        [SerializeField] private float _timeScreenLoadingStay;
        [SerializeField] private float _intervalLoadingPoint;
        
        [Header("References")]
        [SerializeField] private Image _screenEnsi;
        [SerializeField] private Image _illustration;
        [SerializeField] private TextMeshProUGUI _textChargement;
        [SerializeField] private SpriteRenderer _animationChargement;

        private int _currentDots = 0;
        private float _timer;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            _screenEnsi.DOFade(1f, _fadeInDuration);
            Invoke(nameof(FadeOutEnsi), _timeScreenEnsiStay);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _intervalLoadingPoint)
            {
                _timer = 0f;
                DisplayLoandingPoints();
            }
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
            Invoke(nameof(ChangeScene), _timeScreenLoadingStay);
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(1);
        }

        private void DisplayLoandingPoints()
        {
            _currentDots = (_currentDots + 1) % 4; // Boucle entre 0, 1, 2, 3
            switch (_currentDots)
            {
                case 0:
                    _textChargement.text = "Chargement";
                    break;
                case 1:
                    _textChargement.text = "Chargement.";
                    break;
                case 2:
                    _textChargement.text = "Chargement..";
                    break;
                case 3:
                    _textChargement.text = "Chargement...";
                    break;
            }
        }
    }
}
