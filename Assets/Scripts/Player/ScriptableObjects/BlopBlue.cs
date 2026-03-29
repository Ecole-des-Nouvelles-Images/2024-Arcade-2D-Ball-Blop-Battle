using Balls;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlopBleu", menuName = "PlayerData/BlopBleu")]
    public class BlopBlue : Blop
    {
        // Ball Components
        private PlayerController _playerController;
        private BallController _ballController;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" BLEU : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
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
            _playerController.ResetSpecialSpikeState();
        }
    }
}