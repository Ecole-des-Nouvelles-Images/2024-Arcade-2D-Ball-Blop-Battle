using Hugo.Prototype.Scripts.Ball;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopJaune : PlayerData
    {
        private Rigidbody2D _rb2dBall;
        private Collider2D _col2dBall;
        private BallHandler _ballHandler;

        [Header("Yellow Player Data")]
        [SerializeField] private float _transparentTimer;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" JAUNE : SPECIAL SPIKE ! ");
            
            // Get Componment
            _rb2dBall = ball.GetComponent<Rigidbody2D>();
            _col2dBall = ball.GetComponent<Collider2D>();
            _ballHandler = ball.GetComponent<BallHandler>();

            // Change Contraints
            _rb2dBall.constraints = RigidbodyConstraints2D.None;
            _rb2dBall.constraints = RigidbodyConstraints2D.FreezeRotation;
            _ballHandler.ReversIsCatch();
            _ballHandler.InvokeMethodTimer("ReverseIsTrigger", 0.1f);
            
            // Special Spike
            _rb2dBall.AddForce(direction * SpeedSpecialSpike, ForceMode2D.Impulse);
            _ballHandler.InvokeMethodTimer("IsTransparent", _transparentTimer);
        }
    }
}