using Hugo.Refacto.Scripts;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRouge : Blop
    {
        // Ball Components
        private NewBlopController _newBlopController;
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" RED : SPECIAL SPIKE ! ");
            
            // Get Components
            _newBlopController = player.GetComponent<NewBlopController>();
            _ballHandler = ball.GetComponent<BallController>();
            
            GameObject playerSpecialSpike = Instantiate(PlayerSpecialSpike, player.transform.position, 
                player.transform.rotation, player.transform);
            
            playerSpecialSpike.GetComponent<PlayerSpecialSpikeRed>().Setup(_newBlopController, _ballHandler, 
                direction, SpeedSpecialSpike);
        }
    }
}