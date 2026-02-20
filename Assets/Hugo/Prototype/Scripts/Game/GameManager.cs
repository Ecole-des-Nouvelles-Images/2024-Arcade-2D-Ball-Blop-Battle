using System.Collections.Generic;
using Hugo.Prototype.Scripts.Player;
using Int.Scripts.Utils.Singletons;
using UnityEngine;

namespace Hugo.Prototype.Scripts.Game
{
    public class GameManager : MonoBehaviourSingletonDontDestroyOnLoad<GameManager>
    {
        // Players
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        public static Blop FirstBlopScriptableObject;
        public static Blop SecondBlopScriptableObject;
        
        // List DevicesID
        public static List<int> DevicesID = new List<int>();
        
        // States of Game
        public static bool HasGameLoaded = false;
        public static bool IsGamePaused = false;
    }
}