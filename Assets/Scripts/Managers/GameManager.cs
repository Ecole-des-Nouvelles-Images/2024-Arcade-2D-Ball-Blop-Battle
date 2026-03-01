using System.Collections.Generic;
using Player.ScriptableObjects;
using UnityEngine;
using Utils.Singletons;

namespace Managers
{
    public class GameManager : MonoBehaviourSingletonDontDestroyOnLoad<GameManager>
    {
        // Players
        public GameObject FirstPlayerGameObject;
        public GameObject SecondPlayerGameObject;
        public Blop FirstBlopScriptableObject;
        public Blop SecondBlopScriptableObject;
        
        // List DevicesID
        public List<int> DevicesID = new();
        
        // States of Game
        public static bool HasGameLoaded = false;
        public static bool IsGamePaused = false;
    }
}