using Hugo.Refacto.Scripts;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopVert", menuName = "PlayerData/BlopVert")]
    public class BlopVert : Blop
    {
        // Ball Components
        private NewBlopController _newBlopController;
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" GREEN : SPECIAL SPIKE ! ");
            
            // Get Components
            _newBlopController = player.GetComponent<NewBlopController>();
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
            
            _newBlopController.ResetSpecialSpikeState();
        }
    }
}