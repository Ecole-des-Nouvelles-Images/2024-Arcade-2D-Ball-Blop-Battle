using System.Collections;
using _Branches.Hugo.OldScripts.Game;
using _Branches.Hugo.OldScripts.Player;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Branches.Hugo.OldScripts.UI
{
    public class HUDDisplay : MonoBehaviour
    {
        private RectTransform _rectTransformScorePlayerOne;
        private RectTransform _rectTransformScorePlayerTwo;
        
        private Color _colorEmptyPerfectReception = Color.gray;
        private Color _colorFillPerfectReception = new Color(1f, 0.6f, 0.2f);
        private Color _colorReadyPerfectReception = new Color(0f, 1f, 0f);
        
        [Header("Game Manager")]
        [SerializeField] private GameManager _gameManager;
        [FormerlySerializedAs("_matchManager")] [SerializeField] private OldMatchManager oldMatchManager;
        
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI _textTimer;
        [SerializeField] private TextMeshProUGUI _textScorePlayerOne;
        [SerializeField] private TextMeshProUGUI _textScorePlayerTwo;
        [SerializeField] private Image _firstPerfectReceptionPlayerOne;
        [SerializeField] private Image _secondPerfectReceptionPlayerOne;
        [SerializeField] private Image _thirdPerfectReceptionPlayerOne;
        [SerializeField] private Image _firstPerfectReceptionPlayerTwo;
        [SerializeField] private Image _secondPerfectReceptionPlayerTwo;
        [SerializeField] private Image _thirdPerfectReceptionPlayerTwo;
        [SerializeField] private Image _firstSetPlayerOne;
        [SerializeField] private Image _secondSetPlayerOne;
        [SerializeField] private Image _thirdSetPlayerOne;
        [SerializeField] private Image _firstSetPlayerTwo;
        [SerializeField] private Image _secondSetPlayerTwo;
        [SerializeField] private Image _thirdSetPlayerTwo;

        [Header("Settings Scale Score")]
        [SerializeField] private Vector3 _initialScale;
        [SerializeField] private Vector3 _targetScale;
        
        [Header("Pressed B")]
        [SerializeField] private GameObject _displayPressB;
        private GameObject _displayPressBGameObjectPlayerOne;
        private GameObject _displayPressBGameObjectPlayerTwo;

        private void Awake()
        {
            _rectTransformScorePlayerOne = _textScorePlayerOne.gameObject.GetComponent<RectTransform>();
            _rectTransformScorePlayerTwo = _textScorePlayerTwo.gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Timer and Scoring
            _textScorePlayerOne.text = OldMatchManager.ScorePlayerOne.ToString();
            _textScorePlayerTwo.text = OldMatchManager.ScorePlayerTwo.ToString();
            _textTimer.text = Mathf.RoundToInt(OldMatchManager.CurrentTime).ToString();
            
            // Perfect Reception
            // Player One
            if (_gameManager.FirstPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.FirstPlayerGameObject.GetComponent<OldPlayerController>().PerfectReceptionCount;
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
                    _secondPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
                    _thirdPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
                    
                    if (_displayPressBGameObjectPlayerOne)
                    {
                        Destroy(_displayPressBGameObjectPlayerOne);
                    }
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerOne.color = _colorFillPerfectReception;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerOne.color = _colorFillPerfectReception;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerOne.color = _colorReadyPerfectReception;
                    _secondPerfectReceptionPlayerOne.color = _colorReadyPerfectReception;
                    _thirdPerfectReceptionPlayerOne.color = _colorReadyPerfectReception;
                    
                    if (!_displayPressBGameObjectPlayerOne)
                    {
                        _displayPressBGameObjectPlayerOne = Instantiate(_displayPressB, new Vector3(-3.75f, 4.25f, 0), Quaternion.identity);
                        
                        Canvas canvas = _displayPressBGameObjectPlayerOne.GetComponent<Canvas>();
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.sortingOrder = 25;
                        
                        Image image = canvas.GetComponentInChildren<Image>();
                        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0.31f, 0.88f);
                        rectTransform.anchorMax = new Vector2(0.36f, 0.98f);
                    }
                }
            }
            else
            {
                _firstPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
                _secondPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
                _thirdPerfectReceptionPlayerOne.color = _colorEmptyPerfectReception;
            }
            
            // Player Two
            if (_gameManager.SecondPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.SecondPlayerGameObject.GetComponent<OldPlayerController>().PerfectReceptionCount;
                
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
                    _secondPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
                    _thirdPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
                    
                    if (_displayPressBGameObjectPlayerTwo)
                    {
                        Destroy(_displayPressBGameObjectPlayerTwo);
                    }
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerTwo.color = _colorFillPerfectReception;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerTwo.color = _colorFillPerfectReception;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerTwo.color = _colorReadyPerfectReception;
                    _secondPerfectReceptionPlayerTwo.color = _colorReadyPerfectReception;
                    _thirdPerfectReceptionPlayerTwo.color = _colorReadyPerfectReception;

                    if (!_displayPressBGameObjectPlayerTwo)
                    {
                        _displayPressBGameObjectPlayerTwo = Instantiate(_displayPressB, new Vector3(3.75f, 4.25f, 0), Quaternion.identity);
                        
                        Canvas canvas = _displayPressBGameObjectPlayerTwo.GetComponent<Canvas>();
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.sortingOrder = 25;

                        Image image = canvas.GetComponentInChildren<Image>();
                        RectTransform rectTransform = image.gameObject.GetComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0.64f, 0.88f);
                        rectTransform.anchorMax = new Vector2(0.69f, 0.98f);
                    }
                }
            }
            else
            {
                _firstPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
                _secondPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
                _thirdPerfectReceptionPlayerTwo.color = _colorEmptyPerfectReception;
            }
            
            // Set Display
            if (oldMatchManager)
            {
                // Player One
                int setCountPlayerOne = oldMatchManager.SetScorePlayerOne;
                if (setCountPlayerOne == 0)
                {
                    _firstSetPlayerOne.color = Color.gray;
                    _secondSetPlayerOne.color = Color.gray;
                    _thirdSetPlayerOne.color = Color.gray;
                }
                
                if (setCountPlayerOne == 1)
                {
                    _firstSetPlayerOne.color = Color.green;
                }

                if (setCountPlayerOne == 2)
                {
                    _secondSetPlayerOne.color = Color.green;
                }

                if (setCountPlayerOne == 3)
                {
                    _thirdSetPlayerOne.color = Color.green;
                }
                
                // Player One
                int setCountPlayerTwo = oldMatchManager.SetScorePlayerTwo;
                if (setCountPlayerTwo == 0)
                {
                    _firstSetPlayerTwo.color = Color.gray;
                    _secondSetPlayerTwo.color = Color.gray;
                    _thirdSetPlayerTwo.color = Color.gray;
                }
                
                if (setCountPlayerTwo == 1)
                {
                    _firstSetPlayerTwo.color = Color.green;
                }

                if (setCountPlayerTwo == 2)
                {
                    _secondSetPlayerTwo.color = Color.green;
                }

                if (setCountPlayerTwo == 3)
                {
                    _thirdSetPlayerTwo.color = Color.green;
                }
            }
        }

        public void DisplayScoreChange(bool playerOneScored)
        {
            if (playerOneScored)
            {
                _rectTransformScorePlayerOne.transform.localScale = _targetScale;
                StartCoroutine(ResetScale(_rectTransformScorePlayerOne, 1.5f));
            }
            else
            {
                _rectTransformScorePlayerTwo.localScale = _targetScale;
                StartCoroutine(ResetScale(_rectTransformScorePlayerTwo, 1.5f));
            }
        }

        private IEnumerator ResetScale(RectTransform score, float delay)
        {
            yield return new WaitForSeconds(delay);

            score.localScale = _initialScale;
        }
    }
}