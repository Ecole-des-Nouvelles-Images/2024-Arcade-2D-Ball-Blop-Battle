using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopViolet", menuName = "PlayerData/BlopViolet")]
    public class BlopViolet : PlayerType
    {
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}