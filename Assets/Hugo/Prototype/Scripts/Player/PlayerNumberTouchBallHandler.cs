using Hugo.Prototype.Scripts.Game;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    public class PlayerNumberTouchBallHandler : MonoBehaviour
    {
        public int NumberTouchBall;
        public bool IsPlayerOne;

        private GameObject _ballGameObject;

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

        public void Fouls()
        {
            Debug.Log(" FAUTE ");
            if (IsPlayerOne)
            {
                MatchManager.ScorePlayerTwo++;
                MatchManager.PlayerOneScoreLast = true;
            }
            else
            {
                MatchManager.ScorePlayerOne++;
                MatchManager.PlayerOneScoreLast = false;
            }
                    
            NumberTouchBall = 0;
            Destroy(_ballGameObject);
        }
    }
}
