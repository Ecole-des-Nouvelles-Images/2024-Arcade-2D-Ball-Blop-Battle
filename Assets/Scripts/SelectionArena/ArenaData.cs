using UnityEngine;

namespace SelectionArena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "ScriptableObject/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        [Header("===== REFERENCES =====")]
        public string ArenaName;
        public Sprite ArenaVisual;
        public string ArenaSceneName;
    }
}