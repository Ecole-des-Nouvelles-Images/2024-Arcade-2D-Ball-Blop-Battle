using Balls;
using SpecialSpikes;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlopVert", menuName = "PlayerData/BlopVert")]
    public class BlopGreen : Blop
    {
        // Ball Components
        private PlayerController _playerController;
        private MatchBallController _matchBallHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" GREEN : SPECIAL SPIKE ! ");
            
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
            
            GameObject playerSpecialSpike = Instantiate(PlayerSpecialSpike, player.transform.position, player.transform.rotation, 
                player.transform);
            GameObject ballSpecialSpike = Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation, 
                ball.transform);
            
            playerSpecialSpike.GetComponent<PlayerSpecialSpikeGreen>().Setup(ballSpecialSpike.GetComponent<BallSpecialSpikeGreen>());
            ballSpecialSpike.GetComponent<BallSpecialSpikeGreen>().Setup(playerSpecialSpike.GetComponent<PlayerSpecialSpikeGreen>());
            
            _playerController.ResetSpecialSpikeState();
        }
    }
}