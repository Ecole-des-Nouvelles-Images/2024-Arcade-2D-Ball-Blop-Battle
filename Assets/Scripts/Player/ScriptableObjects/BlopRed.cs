using Balls;
using SpecialSpikes;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRed : Blop
    {
        // Ball Components
        private PlayerController _playerController;
        private BallController _ballHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" RED : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _ballHandler = ball.GetComponent<BallController>();
            
            GameObject playerSpecialSpike = Instantiate(PlayerSpecialSpike, player.transform.position, 
                player.transform.rotation, player.transform);
            
            playerSpecialSpike.GetComponent<PlayerSpecialSpikeRed>().Setup(_playerController, _ballHandler, 
                direction, SpeedSpecialSpike);
        }
    }
}