using Hugo.Refacto.Scripts;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopJaune", menuName = "PlayerData/BlopJaune")]
    public class BlopJaune : Blop
    {
        // Ball Components
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" YELLOW : SPECIAL SPIKE ! ");
            
            // Get Components
            _ballHandler = ball.GetComponent<BallController>();
            
            // Special Spike
            if (direction == Vector2.zero)
            {
                _ballHandler.DrawnSpacialSpike(direction, SpeedSpecialSpike);
            }
            else
            {
                _ballHandler.DrawnSpacialSpike(direction, SpeedSpecialSpike);
            }
            
            GameObject go = Instantiate(BallSpecialSpike, ball.transform.position, ball.transform.rotation,
                ball.transform);
        }
    }
}