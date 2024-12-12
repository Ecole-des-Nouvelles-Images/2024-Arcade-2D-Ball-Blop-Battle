using System.Collections;
using TMPro;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Arene
{
    public class SignsHandler : MonoBehaviour
    {
        [Header("Panel Settings")]
        [SerializeField] private Color _blinkColor = new Color(1,1,1,0.5f); // Couleur pendant le clignotement
        [SerializeField] private float _blinkDuration = 1f; // Durée totale du clignotement
        [SerializeField] private float _blinkInterval = 0.2f; // Intervalle entre chaque clignotement

        private Color _originalColor; // Couleur d'origine du panneau
        private bool _isBlinking; // Indique si le panneau est en train de clignoter
        private TextMeshPro _panelTextMeshPro;

        private void Awake()
        {
            _panelTextMeshPro = GetComponent<TextMeshPro>();
            _originalColor = _panelTextMeshPro.color;
        }

        private void OnEnable()
        {
            if (!_isBlinking)
            {
                Debug.Log("start coroutine");
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
                _panelTextMeshPro.color = _panelTextMeshPro.color == _originalColor ? _blinkColor : _originalColor;

                // Attendre le prochain clignotement
                yield return new WaitForSeconds(_blinkInterval);

                elapsedTime += _blinkInterval;
            }

            // Réinitialiser à la couleur d'origine
            _panelTextMeshPro.color = _originalColor;
            _isBlinking = false;
            gameObject.SetActive(false);
            
            Debug.Log("reset coroutine");
        }
    }
}
