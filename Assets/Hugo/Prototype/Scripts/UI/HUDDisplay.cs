using System.Collections;
using Hugo.Prototype.Scripts.Game;
using Hugo.Prototype.Scripts.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hugo.Prototype.Scripts.UI
{
    public class HUDDisplay : MonoBehaviour
    {
        private RectTransform _rectTransformScorePlayerOne;
        private RectTransform _rectTransformScorePlayerTwo;
        
        [Header("Game Manager")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private MatchManager _matchManager;
        
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

        private void Awake()
        {
            _rectTransformScorePlayerOne = _textScorePlayerOne.gameObject.GetComponent<RectTransform>();
            _rectTransformScorePlayerTwo = _textScorePlayerTwo.gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            // Timer and Scoring
            _textScorePlayerOne.text = MatchManager.ScorePlayerOne.ToString();
            _textScorePlayerTwo.text = MatchManager.ScorePlayerTwo.ToString();
            _textTimer.text = Mathf.RoundToInt(MatchManager.CurrentTime).ToString();
            
            // Perfect Reception
            // Player One
            if (_gameManager.FirstPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.FirstPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount;
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.white;
                    _secondPerfectReceptionPlayerOne.color = Color.white;
                    _thirdPerfectReceptionPlayerOne.color = Color.white;
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.yellow;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerOne.color = Color.yellow;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerOne.color = Color.red;
                    _secondPerfectReceptionPlayerOne.color = Color.red;
                    _thirdPerfectReceptionPlayerOne.color = Color.red;
                }
            }
            else
            {
                _firstPerfectReceptionPlayerOne.color = Color.white;
                _secondPerfectReceptionPlayerOne.color = Color.white;
                _thirdPerfectReceptionPlayerOne.color = Color.white;
            }
            
            // Player Two
            if (_gameManager.SecondPlayerGameObject)
            {
                int perfectReceptionCount = _gameManager.SecondPlayerGameObject.GetComponent<PlayerController>().PerfectReceptionCount;
                
                if (perfectReceptionCount == 0)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.white;
                    _secondPerfectReceptionPlayerTwo.color = Color.white;
                    _thirdPerfectReceptionPlayerTwo.color = Color.white;
                }
                
                if (perfectReceptionCount == 1)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.yellow;
                }

                if (perfectReceptionCount == 2)
                {
                    _secondPerfectReceptionPlayerTwo.color = Color.yellow;
                }

                if (perfectReceptionCount == 3)
                {
                    _firstPerfectReceptionPlayerTwo.color = Color.red;
                    _secondPerfectReceptionPlayerTwo.color = Color.red;
                    _thirdPerfectReceptionPlayerTwo.color = Color.red;
                }
            }
            else
            {
                _firstPerfectReceptionPlayerTwo.color = Color.white;
                _secondPerfectReceptionPlayerTwo.color = Color.white;
                _thirdPerfectReceptionPlayerTwo.color = Color.white;
            }
            
            // Set Display
            if (_matchManager)
            {
                // Player One
                int setCountPlayerOne = _matchManager.SetScorePlayerOne;
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
                int setCountPlayerTwo = _matchManager.SetScorePlayerTwo;
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