using System;
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
        
        private void Awake()
        {
            _matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Ball"))
            {
                _ballGameObject = other.gameObject;
                NumberTouchBall++;

                if (NumberTouchBall > 2)
                {
                    Fouls();
                }
            }
        }

        private void Update()
        {
            if (IsPlayerOne)
            {
                Debug.Log(NumberTouchBall);
            }
        }

        public void Fouls()
        {
            //Debug.Log(" FAUTE ");
            if (IsPlayerOne)
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
                _matchManager.DisplayScoreChange(false);
            }
            else
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
                _matchManager.DisplayScoreChange(true);
            }
                    
            NumberTouchBall = 0;
            Destroy(_ballGameObject);
        }
    }
}
