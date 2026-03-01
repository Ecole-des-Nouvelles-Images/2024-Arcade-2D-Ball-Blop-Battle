using Hugo.Refacto.Scripts;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopBleu", menuName = "PlayerData/BlopBleu")]
    public class BlopBleu : Blop
    {
        // Ball Components
        private NewBlopController _newBlopController;
        private BallController _ballController;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" BLEU : SPECIAL SPIKE ! ");
            
            // Get Components
            _newBlopController = player.GetComponent<NewBlopController>();
            _ballController = ball.GetComponent<BallController>();
            
            // Special Spike
            if (direction == Vector2.zero)
            {
                _ballController.DrawnSpecialSpike(Vector2.up, SpeedSpecialSpike);
            }
            else
            {
                _ballController.DrawnSpecialSpike(direction, SpeedSpecialSpike);
            }
            
            Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation, ball.transform);
            _newBlopController.ResetSpecialSpikeState();
        }
    }
}