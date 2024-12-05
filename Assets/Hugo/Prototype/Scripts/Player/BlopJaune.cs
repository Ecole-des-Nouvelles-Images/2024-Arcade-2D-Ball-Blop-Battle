using Hugo.Prototype.Scripts.Ball;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopJaune : PlayerType
    {
        // Ball Components
        private PlayerController _playerController;
        private Rigidbody2D _rb2dBall;
        private BallHandler _ballHandler;

        [FormerlySerializedAs("_transparentTimer")]
        [Header("Yellow Player Data")]
        [SerializeField] private float _becameTransparentTimer;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" JAUNE : SPECIAL SPIKE ! ");
            
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
            else
            {
                _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
            }
            _ballHandler.InvokeMethodTimer("YellowSpecialSpike", _becameTransparentTimer);
            
            _playerController.ResetStatesAfterSpecialSpike();
        }
    }
}