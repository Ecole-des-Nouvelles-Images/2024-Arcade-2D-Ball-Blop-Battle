using UnityEngine;

namespace Hugo.Prototype.Scripts
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