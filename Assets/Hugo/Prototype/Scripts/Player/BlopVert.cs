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

            // Change Constraints
            _rb2dBall.constraints = RigidbodyConstraints2D.None;
            _rb2dBall.constraints = RigidbodyConstraints2D.FreezeRotation;
            _ballHandler.ReversIsCatch();
            _ballHandler.InvokeMethodTimer("ReverseIsTrigger", 0.1f);
            
            // Special Spike
            if (direction == Vector2.zero)
            {
                _rb2dBall.AddForce(new Vector2(1,0) * SpeedSpecialSpike, ForceMode2D.Impulse);
            }
            _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
            
            _playerController.GreenSpecialSpike = true;
        }
    }
}