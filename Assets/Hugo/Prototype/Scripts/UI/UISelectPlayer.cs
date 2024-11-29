using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class UISelectPlayer : MonoBehaviour
    {
        public bool IsPlayerOne;
        
        private PlayerInput _playerInput;
        private Image _currentSelectedBlopImage;
        private RectTransform _currentSelectedBlopRectTransform;
        private GameObject _currentButtonSelected;
        private bool _hasGameLoaded;

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
            _currentButtonSelected = _eventSystem.currentSelectedGameObject;
            
            SelectionBlop();

            if (GameManager.FirstPlayerScriptableObject != null && GameManager.SecondPlayerScriptableObject != null && !_hasGameLoaded)
            {
                Debug.Log(" Start Game ! ");
                _hasGameLoaded = true;
                Invoke(nameof(LoadGameScene), 0.2f);
            }
        }

        private void SelectionBlop()
        {
            if (_currentButtonSelected)
            {
                if (_currentButtonSelected.name == "Bleu")
                {
                    _currentSelectedBlopImage.sprite = _bleuBlop.Sprite;

                    if (_currentButtonSelected.GetComponent<Button>().onClick != null)
                    {
                        if (IsPlayerOne)
                        {
                            GameManager.FirstPlayerScriptableObject = _bleuBlop;
                        }
                        else
                        {
                            GameManager.SecondPlayerScriptableObject = _bleuBlop;
                        }
                    }
                }
                else if (_currentButtonSelected.name == "Jaune")
                {
                    _currentSelectedBlopImage.sprite = _jauneBlop.Sprite;

                    if (_currentButtonSelected.GetComponent<Button>().onClick != null)
                    {
                        if (IsPlayerOne)
                        {
                            GameManager.FirstPlayerScriptableObject = _jauneBlop;
                        }
                        else
                        {
                            GameManager.SecondPlayerScriptableObject = _jauneBlop;
                        }
                    }
                }
                else if (_currentButtonSelected.name == "Rouge")
                {
                    _currentSelectedBlopImage.sprite = _rougeBlop.Sprite;

                    if (_currentButtonSelected.GetComponent<Button>().onClick != null)
                    {
                        if (IsPlayerOne)
                        {
                            GameManager.FirstPlayerScriptableObject = _rougeBlop;
                        }
                        else
                        {
                            GameManager.SecondPlayerScriptableObject = _rougeBlop;
                        }
                    }
                }
                else if (_currentButtonSelected.name == "Violet")
                {
                    _currentSelectedBlopImage.sprite = _violetBlop.Sprite;

                    if (_currentButtonSelected.GetComponent<Button>().onClick != null)
                    {
                        if (IsPlayerOne)
                        {
                            GameManager.FirstPlayerScriptableObject = _violetBlop;
                        }
                        else
                        {
                            GameManager.SecondPlayerScriptableObject = _violetBlop;
                        }
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
