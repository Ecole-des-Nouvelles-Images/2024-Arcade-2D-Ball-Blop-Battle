using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopBleu", menuName = "PlayerData/BlopBleu")]
    public class BlopBleu : PlayerData
    {
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}