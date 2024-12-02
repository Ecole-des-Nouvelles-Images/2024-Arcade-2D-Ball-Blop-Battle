using System.Collections.Generic;
using Hugo.Prototype.Scripts.Player;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        
        public static PlayerType FirstPlayerScriptableObject;
        public static PlayerType SecondPlayerScriptableObject;
        
        public static List<int> DevicesID = new List<int>();
        
        public static bool HasGameLoaded = false;

        private void Update()
        {
            Debug.Log(FirstPlayerScriptableObject);
            Debug.Log(SecondPlayerScriptableObject);
        }
    }
}