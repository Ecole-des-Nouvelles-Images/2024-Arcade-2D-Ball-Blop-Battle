using DG.Tweening;
using Managers;
using Player.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

namespace SelectionCharacter
{
    public class CharacterSelectionController : MonoBehaviour
    {
        public bool IsPlayerOne;
        
        [Header("Blops")]
        [SerializeField] private Blop _bleuBlop;
        [SerializeField] private Blop _rougeBlop;
        [SerializeField] private Blop _jauneBlop;
        [SerializeField] private Blop _vertBlop;
        
        [Header("Buttons")]
        [SerializeField] private Image _bleuBlopButton;
        [SerializeField] private Image _rougeBlopButton;
        [SerializeField] private Image _jauneBlopButton;
        [SerializeField] private Image _vertBlopButton;
        
        [Header("Links")]
        [SerializeField] private RectTransform _playerSelection;
        [SerializeField] private RectTransform _specialSpikeText;
        [SerializeField] private GameObject _dislpayCurrentSelectedBlopGameObject;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private TextMeshProUGUI _textSpecialSpike;
        
        [Header("Sprites")]
        [SerializeField] private Sprite _selectedBlopPlayerOne;
        [SerializeField] private Sprite _selectedBlopPlayerTwo;
        
        // Components
        private PlayerInput _playerInput;
        private Image _currentSelectedBlopImage;
        private RectTransform _currentSelectedBlopRectTransform;
        private GameObject _currentButtonSelected;
        
        // Navigation
        private Navigation.Mode _disableNavigation = Navigation.Mode.None;
        private Navigation.Mode _enableNavigation = Navigation.Mode.Explicit;

        private void Awake()
        {
            // Get Components
            _playerInput = GetComponent<PlayerInput>();
            _currentSelectedBlopImage = _dislpayCurrentSelectedBlopGameObject.GetComponent<Image>();
            _currentSelectedBlopRectTransform = _dislpayCurrentSelectedBlopGameObject.GetComponent<RectTransform>();

            if (IsPlayerOne)
            {
                _bleuBlopButton.sprite = _selectedBlopPlayerOne;
                _rougeBlopButton.sprite = _selectedBlopPlayerOne;
                _jauneBlopButton.sprite = _selectedBlopPlayerOne;
                _vertBlopButton.sprite = _selectedBlopPlayerOne;
            }
            else
            {
                _bleuBlopButton.sprite = _selectedBlopPlayerTwo;
                _rougeBlopButton.sprite = _selectedBlopPlayerTwo;
                _jauneBlopButton.sprite = _selectedBlopPlayerTwo;
                _vertBlopButton.sprite = _selectedBlopPlayerTwo;
            }
        }

        private void OnEnable()
        {
            // Instantiate Prefab
            if (_playerInput.playerIndex == 0)
            {
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.15f, 0.45f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.35f, 0.85f);
                
                _specialSpikeText.anchorMin = new Vector2(0f, 0.02f);
                _specialSpikeText.anchorMax = new Vector2(0.25f, 0.4f);
            }
            else
            {
                _currentSelectedBlopRectTransform.rotation = Quaternion.Euler(0f, 180f, 0f);
                
                _currentSelectedBlopRectTransform.anchorMin = new Vector2(0.65f, 0.45f);
                _currentSelectedBlopRectTransform.anchorMax = new Vector2(0.85f, 0.85f);
                
                _specialSpikeText.anchorMin = new Vector2(0.75f, 0.02f);
                _specialSpikeText.anchorMax = new Vector2(1f, 0.4f);
            }
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
                        _textSpecialSpike.text = _bleuBlop.SpecialSpikeDescription;
                        SubmitBlop(_bleuBlop);
                        break;
                    }
                    case "Jaune":
                    {
                        _currentSelectedBlopImage.sprite = _jauneBlop.Sprite;
                        _textSpecialSpike.text = _jauneBlop.SpecialSpikeDescription;
                        SubmitBlop(_jauneBlop);
                        break;
                    }
                    case "Rouge":
                    {
                        _currentSelectedBlopImage.sprite = _rougeBlop.Sprite;
                        _textSpecialSpike.text = _rougeBlop.SpecialSpikeDescription;
                        SubmitBlop(_rougeBlop);
                        break;
                    }
                    case "Vert": 
                    {
                        _currentSelectedBlopImage.sprite = _vertBlop.Sprite;
                        _textSpecialSpike.text = _vertBlop.SpecialSpikeDescription;
                        SubmitBlop(_vertBlop);
                        break;
                    }
                }
            }
        }

        private void SubmitBlop(Blop blop)
        {
            if (_playerInput.actions["UI/Submit"].triggered)
            {
                if (IsPlayerOne)
                {
                    GameManager.Instance.Players.Add(new PlayerData(1, _playerInput.devices[0].deviceId, blop));
                }
                else
                {
                    GameManager.Instance.Players.Add(new PlayerData(2, _playerInput.devices[0].deviceId, blop));
                }
                
                _dislpayCurrentSelectedBlopGameObject.transform.DOScale(1.2f, 0.3f).SetEase(Ease.OutBounce);
                _currentButtonSelected.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1f);
                
                var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                navigation.mode = _disableNavigation;
                _currentButtonSelected.GetComponent<Button>().navigation = navigation;
            }
        }

        private void CancelBlopSelection()
        {
            if (_playerInput.actions["UI/Cancel"].triggered)
            {
                PlayerData player = GameManager.Instance.Players.Find(p => p.DeviceId == _playerInput.devices[0].deviceId);

                if (player == null)
                {
                    GameManager.Instance.Players.Clear();
                    SceneLoaderManager.Instance.LoadScene("MainMenu");
                    return;
                }
                
                GameManager.Instance.Players.Remove(player);
                
                _dislpayCurrentSelectedBlopGameObject.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
                _currentButtonSelected.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                _textSpecialSpike.text = null;
                
                var navigation = _currentButtonSelected.GetComponent<Button>().navigation;
                navigation.mode = _enableNavigation;
                _currentButtonSelected.GetComponent<Button>().navigation = navigation;
            }
        }
    }
}
