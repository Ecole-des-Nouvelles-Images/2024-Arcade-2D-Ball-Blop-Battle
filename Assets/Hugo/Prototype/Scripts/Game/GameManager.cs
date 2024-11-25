using Hugo.Prototype.Scripts.Player;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        
        public static PlayerData FirstPlayerScriptableObject;
        public static PlayerData SecondPlayerScriptableObject;
    }
}