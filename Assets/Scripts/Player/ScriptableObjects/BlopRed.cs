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
        private MatchBallController _matchBallHandler;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" RED : SPECIAL SPIKE ! ");
            
            // Get Components
            _playerController = player.GetComponent<PlayerController>();
            _matchBallHandler = ball.GetComponent<MatchBallController>();
            
            GameObject playerSpecialSpike = Instantiate(PlayerSpecialSpike, player.transform.position, 
                player.transform.rotation, player.transform);
            
            playerSpecialSpike.GetComponent<PlayerSpecialSpikeRed>().Setup(_playerController, _matchBallHandler, 
                direction, SpeedSpecialSpike);
        }
    }
}