using Hugo.Prototype.Scripts.Player;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        
        public static PlayerData FirstPlayerScriptableObject;
        public static PlayerData SecondPlayerScriptableObject;
    }
}