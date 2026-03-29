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
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" GREEN : SPECIAL SPIKE ! ");
            
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