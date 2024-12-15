using Hugo.Prototype.Scripts.Ball;
using Hugo.Prototype.Scripts.Camera;
using Hugo.Prototype.Scripts.Game;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerNumberTouchBallHandler : MonoBehaviour
    {
        public int NumberTouchBall;
        public bool IsPlayerOne;

        private GameObject _ballGameObject;
        private MatchManager _matchManager;
        private CameraHandler _cameraHandler;
        private bool _alreadyTouched;
        
        private void Awake()
        {
            _matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
            _cameraHandler = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraHandler>();
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
                }

                if (NumberTouchBall > 2)
                {
                    Fouls();
                }
            }
        }

        public void Fouls()
        {
            if (IsPlayerOne)
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
                _matchManager.DisplayScoreChange(false, true);
                
                _cameraHandler.ScoredShake();
            }
            else
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                _matchManager.DisplayScoreChange(true, true);
                
                _cameraHandler.ScoredShake();
            }
                    
            NumberTouchBall = 0;
            _ballGameObject.GetComponent<BallHandler>().Destroy();
        }

        private void ReverseAlreadyTouched()
        {
            _alreadyTouched = !_alreadyTouched;
        }
    }
}
