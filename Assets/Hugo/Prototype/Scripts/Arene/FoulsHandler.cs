using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.Arene
{
    public class FoulsHandler : MonoBehaviour
    {
        [Header("Panel Settings")]
        [SerializeField] private Color _blinkColor = new Color(1,1,1,0.5f); // Couleur pendant le clignotement
        [SerializeField] private float _blinkDuration = 1f; // Durée totale du clignotement
        [SerializeField] private float _blinkInterval = 0.2f; // Intervalle entre chaque clignotement

        private Color _originalColor; // Couleur d'origine du panneau
        private bool _isBlinking; // Indique si le panneau est en train de clignoter
        private Image _panelImage;

        private void Awake()
        {
            _panelImage = GetComponent<Image>();
            _originalColor = _panelImage.color;
        }

        private void OnEnable()
        {
            if (!_isBlinking)
            {
                StartCoroutine(BlinkCoroutine());
            }
        }

        private IEnumerator BlinkCoroutine()
        {
            _isBlinking = true;
            float elapsedTime = 0f;

            while (elapsedTime < _blinkDuration)
            {
                // Alterner entre la couleur d'origine et la couleur de clignotement
                _panelImage.color = _panelImage.color == _originalColor ? _blinkColor : _originalColor;

                // Attendre le prochain clignotement
                yield return new WaitForSeconds(_blinkInterval);

                elapsedTime += _blinkInterval;
            }

            // Réinitialiser à la couleur d'origine
            _panelImage.color = _originalColor;
            _isBlinking = false;
            gameObject.SetActive(false);
        }
    }
}
