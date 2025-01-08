using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class HUDPressBToShootSpecialSpikeHandler : MonoBehaviour
    {
        private Image _image;
        
        [Header("Settings")]
        [SerializeField] private Sprite _pressedBSprite;
        [SerializeField] private Sprite _releasedBSprite;
        [SerializeField] private float _interval;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            StartCoroutine(Coroutine());
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (_image.sprite == _pressedBSprite)
                {
                    _image.sprite = _releasedBSprite;
                }
                else
                {
                    _image.sprite = _pressedBSprite;
                }
                yield return new WaitForSeconds(_interval);
            }
        }
    }
}
