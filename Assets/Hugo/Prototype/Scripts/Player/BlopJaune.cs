using Hugo.Refacto.Scripts;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopJaune : Blop
    {
        // Ball Components
        private NewBlopController _newBlopController;
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" YELLOW : SPECIAL SPIKE ! ");
            
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
            
            Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation, ball.transform);
            _newBlopController.ResetSpecialSpikeState();
        }
    }
}