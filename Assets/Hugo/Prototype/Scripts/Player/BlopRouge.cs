using UnityEngine;

namespace Hugo.Prototype.Scripts.Player
{
    [CreateAssetMenu(fileName = "BlopRouge", menuName = "PlayerData/BlopRouge")]
    public class BlopRouge : PlayerType
    {
        [SerializeField]
        private GameObject _fakeBallPrefab;
        
        public override void SpecialSpike(GameObject player, GameObject ball, Vector2 direction)
        {
            Debug.Log(" ROUGE : SPECIAL SPIKE ! ");
        }
    }
}