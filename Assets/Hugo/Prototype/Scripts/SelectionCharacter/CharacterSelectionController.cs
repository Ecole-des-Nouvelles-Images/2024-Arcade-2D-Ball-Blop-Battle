using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.SelectionCharacter
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public bool IsPlayerOne;
        
        // Components
        private PlayerInput _playerInput;
        private Image _currentSelectedBlopImage;
        private RectTransform _currentSelectedBlopRectTransform;
        private GameObject _currentButtonSelected;
        
        // Navigation
        private Navigation.Mode _disableNavigation = Navigation.Mode.None;
        private Navigation.Mode _enableNavigation = Navigation.Mode.Explicit;

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
            // Get Components
            _playerInput = GetComponent<PlayerInput>();
            _currentSelectedBlopImage = _dislpayCurrentSelectedBlopGameObject.GetComponent<Image>();
            _currentSelectedBlopRectTransform = _dislpayCurrentSelectedBlopGameObject.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            // Instantiate Prefab
            if (_playerInput.playerIndex == 0)
            {
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.2f, 0.5f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.4f, 0.9f);
                
                // Clear deicesID
                GameManager.DevicesID.Clear();
            }
            else
            {
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.6f, 0.5f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.8f, 0.9f);
            }
            
            // Stock deicesID
            GameManager.DevicesID.Add(_playerInput.devices[0].deviceId);
        }

        private void Update()
        {
            _currentButtonSelected = _eventSystem.currentSelectedGameObject;
            SelectionBlop();
            CancelBlopSelection();
        }

        private void SelectionBlop()
        {
            if (_currentButtonSelected)
            {
                switch (_currentButtonSelected.name)
                {
                    case "Bleu":
                    {
                        _currentSelectedBlopImage.sprite = _bleuBlop.Sprite;
                        SubmitBlop(_bleuBlop);
                        break;
                    }
                    case "Jaune":
                    {
                        _currentSelectedBlopImage.sprite = _jauneBlop.Sprite;
                        SubmitBlop(_jauneBlop);
                        break;
                    }
                    case "Rouge":
                    {
                        _currentSelectedBlopImage.sprite = _rougeBlop.Sprite;
                        SubmitBlop(_rougeBlop);
                        break;
                    }
                    case "Violet": 
                    {
                        _currentSelectedBlopImage.sprite = _violetBlop.Sprite;
                        SubmitBlop(_violetBlop);
                        break;
                    }
                }
            }
        }

        private void SubmitBlop(PlayerType blop)
        {
            if (_playerInput.actions["UI/Submit"].triggered)
            {
                if (IsPlayerOne)
                {
                    GameManager.FirstPlayerScriptableObject = blop;
                }
                else
                {
                    GameManager.SecondPlayerScriptableObject = blop;
                }
                            
                var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                navigation.mode = _disableNavigation;
                _currentButtonSelected.GetComponent<Button>().navigation = navigation;
            }
        }

        private void CancelBlopSelection()
        {
            if (_playerInput.actions["UI/Cancel"].triggered)
            {
                if (IsPlayerOne)
                {
                    GameManager.FirstPlayerScriptableObject = null;
                }
                else
                {
                    GameManager.SecondPlayerScriptableObject = null;
                }
                
                var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                navigation.mode = _enableNavigation;
                _currentButtonSelected.GetComponent<Button>().navigation = navigation;
            }
        }
    }
}
