using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopVert", menuName = "PlayerData/BlopVert")]
    public class BlopVert : PlayerType
    {
        // Player Components
        private PlayerController _playerController;
        
        // Ball Components
        private Rigidbody2D _rb2dBall;
        private BallHandler _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" VERT : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _rb2dBall = ball.GetComponent<Rigidbody2D>();
            _ballHandler = ball.GetComponent<BallHandler>();
            
            // Special Spike
            if (_playerController.CountShootSpecialSpike == 0)
            {
                // Change Constraints
                _rb2dBall.constraints = RigidbodyConstraints2D.None;
                _rb2dBall.constraints = RigidbodyConstraints2D.FreezeRotation;
                _ballHandler.ReversIsCatch();
                _ballHandler.InvokeMethodTimer("ReverseIsTrigger", 0.1f);
                
                if (direction == Vector2.zero)
                {
                    _rb2dBall.AddForce(new Vector2(1,0) * SpeedSpecialSpike, ForceMode2D.Impulse);
                }
                else
                {
                    _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
                }
                _playerController.CountShootSpecialSpike++;
                _playerController.ResetStatesAfterSpecialSpike();
                return;
            }

            if (_playerController.CountShootSpecialSpike == 1)
            {
                _rb2dBall.velocity = Vector2.zero;
                _rb2dBall.AddForce(Vector2.down * SpeedSpecialSpike, ForceMode2D.Impulse);
                
                _playerController.CountShootSpecialSpike = 0;
            }
        }
    }
}