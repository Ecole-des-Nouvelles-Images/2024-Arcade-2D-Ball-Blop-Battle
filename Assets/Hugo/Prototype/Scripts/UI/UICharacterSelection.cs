using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UICharacterSelection : MonoBehaviour
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

            // Start Game
            if (IsPlayerOne)
            {
                if (GameManager.FirstPlayerScriptableObject && GameManager.SecondPlayerScriptableObject && !GameManager.HasGameLoaded)
                {
                    //Debug.Log(" Start Game ! ");
                    GameManager.HasGameLoaded = true;
                    Invoke(nameof(LoadGameScene), 1f);
                }
            }
            
            // Cancel Choice
            if (_playerInput.actions["UI/Cancel"].triggered)
            {
                //Debug.Log(" Cancel Selection ! ");

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

        private void SelectionBlop()
        {
            if (_currentButtonSelected)
            {
                switch (_currentButtonSelected.name)
                {
                    case "Bleu":
                    {
                        _currentSelectedBlopImage.sprite = _bleuBlop.Sprite;

                        if (_playerInput.actions["UI/Submit"].triggered)
                        {
                            if (IsPlayerOne)
                            {
                                GameManager.FirstPlayerScriptableObject = _bleuBlop;
                            }
                            else
                            {
                                GameManager.SecondPlayerScriptableObject = _bleuBlop;
                            }

                            var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                            navigation.mode = _disableNavigation;
                            _currentButtonSelected.GetComponent<Button>().navigation = navigation;
                        }

                        break;
                    }
                    case "Jaune":
                    {
                        _currentSelectedBlopImage.sprite = _jauneBlop.Sprite;

                        if (_playerInput.actions["UI/Submit"].triggered)
                        {
                            if (IsPlayerOne)
                            {
                                GameManager.FirstPlayerScriptableObject = _jauneBlop;
                            }
                            else
                            {
                                GameManager.SecondPlayerScriptableObject = _jauneBlop;
                            }
                            
                            var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                            navigation.mode = _disableNavigation;
                            _currentButtonSelected.GetComponent<Button>().navigation = navigation;
                        }

                        break;
                    }
                    case "Rouge":
                    {
                        _currentSelectedBlopImage.sprite = _rougeBlop.Sprite;

                        if (_playerInput.actions["UI/Submit"].triggered)
                        {
                            if (IsPlayerOne)
                            {
                                GameManager.FirstPlayerScriptableObject = _rougeBlop;
                            }
                            else
                            {
                                GameManager.SecondPlayerScriptableObject = _rougeBlop;
                            }
                            
                            var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                            navigation.mode = _disableNavigation;
                            _currentButtonSelected.GetComponent<Button>().navigation = navigation;
                        }

                        break;
                    }
                    case "Violet":
                    {
                        _currentSelectedBlopImage.sprite = _violetBlop.Sprite;

                        if (_playerInput.actions["UI/Submit"].triggered)
                        {
                            if (IsPlayerOne)
                            {
                                GameManager.FirstPlayerScriptableObject = _violetBlop;
                            }
                            else
                            {
                                GameManager.SecondPlayerScriptableObject = _violetBlop;
                            }
                            
                            var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                            navigation.mode = _disableNavigation;
                            _currentButtonSelected.GetComponent<Button>().navigation = navigation;
                        }

                        break;
                    }
                }
            }
        }
        
        private void LoadGameScene()
        {
            SceneManager.LoadScene(2);
        }
    }
}
