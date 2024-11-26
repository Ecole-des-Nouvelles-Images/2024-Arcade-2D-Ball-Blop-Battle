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

        [Header("Blops")]
        [SerializeField]
        private PlayerType _bleuBlop;
        [SerializeField]
        private PlayerType _rougeBlop;
        [SerializeField]
        private PlayerType _jauneBlop;
        [SerializeField]
        private PlayerType _violetBlop;
        
        [Header("Links")]
        [SerializeField]
        private RectTransform _playerSelection;
        [SerializeField]
        private Image _dislpayCurrentSelectedBlopImage;
        [SerializeField]
        private EventSystem _eventSystem;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            if (_playerInput.playerIndex == 0)
            {
                _playerSelection.anchorMin = new Vector2(0f, 0f);
                _playerSelection.anchorMax = new Vector2(0.5f, 1f);
            }
            else
            {
                _playerSelection.anchorMin = new Vector2(0.5f, 0f);
                _playerSelection.anchorMax = new Vector2(1f, 1f);
            }
        }

        private void Update()
        {
            
            _dislpayCurrentSelectedBlopImage.sprite = _eventSystem.currentSelectedGameObject.GetComponent<Image>().sprite;
        }
    }
}
