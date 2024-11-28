using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UISelectPlayer : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private Image _currentSelectedBlopImage;
        private RectTransform _currentSelectedBlopRectTransform;

        [Header("Blops")]
        [SerializeField] private PlayerType _bleuBlop;
        [SerializeField] private PlayerType _rougeBlop;
        [SerializeField] private PlayerType _jauneBlop;
        [SerializeField] private PlayerType _violetBlop;
        
        [Header("Links")]
        [SerializeField] private RectTransform _playerSelection;
        [SerializeField] private GameObject _dislpayCurrentSelectedBlopGameObject;
        [SerializeField] private EventSystem _eventSystem;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _currentSelectedBlopImage = _dislpayCurrentSelectedBlopGameObject.GetComponent<Image>();
            _currentSelectedBlopRectTransform = _dislpayCurrentSelectedBlopGameObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (_playerInput.playerIndex == 0)
            {
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.2f, 0.5f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.4f, 0.9f);
            }
            else
            {
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.6f, 0.5f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.8f, 0.9f);
            }
        }

        private void Update()
        {
            if (_eventSystem.currentSelectedGameObject)
            {
                if (_eventSystem.currentSelectedGameObject.name == "Bleu")
                {
                    _currentSelectedBlopImage.sprite = _bleuBlop.Sprite;
                }
                else if (_eventSystem.currentSelectedGameObject.name == "Jaune")
                {
                    _currentSelectedBlopImage.sprite = _jauneBlop.Sprite;
                }
                else if (_eventSystem.currentSelectedGameObject.name == "Rouge")
                {
                    _currentSelectedBlopImage.sprite = _rougeBlop.Sprite;
                }
                else if (_eventSystem.currentSelectedGameObject.name == "Violet")
                {
                    _currentSelectedBlopImage.sprite = _violetBlop.Sprite;
                }
            }
        }
    }
}
