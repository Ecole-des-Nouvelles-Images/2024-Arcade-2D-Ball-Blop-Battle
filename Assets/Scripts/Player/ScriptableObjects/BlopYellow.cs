using Balls;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopYellow : Blop
    {
        // Ball Components
        private PlayerController _playerController;
        private MatchBallController _matchBallHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" YELLOW : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _matchBallHandler = ball.GetComponent<MatchBallController>();
            
            // Special Spike
            if (direction == Vector2.zero)
            {
                _matchBallHandler.DrawnSpecialSpike(Vector2.up, SpeedSpecialSpike);
            }
            else
            {
                _matchBallHandler.DrawnSpecialSpike(direction, SpeedSpecialSpike);
            }
            
            Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation, ball.transform);
            _playerController.ResetSpecialSpikeState();
        }
    }
}