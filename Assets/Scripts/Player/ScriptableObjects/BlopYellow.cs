using Balls;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopYellow : Blop
    {
        // Ball Components
        private PlayerController _playerController;
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" YELLOW : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _ballHandler = ball.GetComponent<BallController>();
            
            // Special Spike
            if (direction == Vector2.zero)
            {
                _ballHandler.DrawnSpecialSpike(Vector2.up, SpeedSpecialSpike);
            }
            else
            {
                _ballHandler.DrawnSpecialSpike(direction, SpeedSpecialSpike);
            }
            
            Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation, ball.transform);
            _playerController.ResetSpecialSpikeState();
        }
    }
}