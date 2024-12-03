using System.Collections.Generic;
using Hugo.Prototype.Scripts.Player;
using Hugo.Prototype.Scripts.Utils;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        // Players
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        public static PlayerType FirstPlayerScriptableObject;
        public static PlayerType SecondPlayerScriptableObject;
        
        // List DevicesID
        public static List<int> DevicesID = new List<int>();
        
        // States of Game
        public static bool HasGameLoaded = false;
        public static bool IsGamePaused = false;
    }
}