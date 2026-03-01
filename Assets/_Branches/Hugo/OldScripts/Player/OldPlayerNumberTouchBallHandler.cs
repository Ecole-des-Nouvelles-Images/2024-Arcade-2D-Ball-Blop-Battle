using _Branches.Hugo.OldScripts.Ball;
using _Branches.Hugo.OldScripts.Game;
using Cam;
using TMPro;
using UnityEngine;

namespace _Branches.Hugo.OldScripts.Player
{
    public class OldPlayerNumberTouchBallHandler : MonoBehaviour
    {
        public int NumberTouchBall;
        public bool IsPlayerOne;
        
        [Header("References")]
        [SerializeField] private GameObject _canvasNumberTouchBallGameObject;
        
        [Header("Celebrations")]
        [SerializeField] private GameObject _celebrationPointObject;
        
        private GameObject _ballGameObject;
        private OldMatchManager _oldMatchManager;
        private CameraHandler _cameraHandler;
        private bool _alreadyTouched;
        private TextMeshProUGUI _textNumberTouchBallText;
        private GameObject _firstCanvas;
        
        private void Awake()
        {
            _oldMatchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<OldMatchManager>();
            _cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();

            _textNumberTouchBallText = _canvasNumberTouchBallGameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ballGameObject = other.gameObject;

                if (!_alreadyTouched)
                {
                    NumberTouchBall++;
                    _alreadyTouched = true;
                    Invoke(nameof(ReverseAlreadyTouched), 0.1f);
                    
                    // Display
                    _textNumberTouchBallText.text = NumberTouchBall.ToString();

                    if (_firstCanvas == null)
                    {
                        _firstCanvas = Instantiate(_canvasNumberTouchBallGameObject, gameObject.transform.position + new Vector3(0.8f, 0.8f, 0f), Quaternion.identity);
                    }
                    else
                    {
                        Destroy(_firstCanvas);
                        _firstCanvas = Instantiate(_canvasNumberTouchBallGameObject, gameObject.transform.position + new Vector3(0.8f, 0.8f, 0f), Quaternion.identity);
                    }
                    
                }

                if (NumberTouchBall > 2)
                {
                    Fouls();
                }
            }
        }

        public void Fouls()
        {
            _oldMatchManager.IsTimerRunning = false;
            
            if (IsPlayerOne)
            {
                OldMatchManager.ScorePlayerTwo++;
                OldMatchManager.PlayerOneScoreLast = true;
                _oldMatchManager.DisplayScoreChange(false, true);
                
                Instantiate(_celebrationPointObject, new Vector3(8, 2, 0), Quaternion.identity);
                
                _cameraHandler.ScoredShake();
            }
            else
            {
                OldMatchManager.ScorePlayerOne++;
                OldMatchManager.PlayerOneScoreLast = false;
                _oldMatchManager.DisplayScoreChange(true, true);
                
                Instantiate(_celebrationPointObject, new Vector3(-8, 2, 0), Quaternion.identity);
                
                _cameraHandler.ScoredShake();
            }
                    
            NumberTouchBall = 0;
            if (_ballGameObject)
            {
                _ballGameObject.GetComponent<OldBallHandler>().Destroy();
            }
        }

        private void ReverseAlreadyTouched()
        {
            _alreadyTouched = !_alreadyTouched;
        }
    }
}
