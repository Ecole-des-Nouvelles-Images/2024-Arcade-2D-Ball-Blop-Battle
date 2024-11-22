using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRouge : PlayerData
    {
        public override void SpecialSpike()
        {
            Debug.Log(" Special Spike ! ");
        }
    }
}