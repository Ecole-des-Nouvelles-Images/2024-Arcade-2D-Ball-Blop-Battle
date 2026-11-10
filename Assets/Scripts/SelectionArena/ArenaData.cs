using UnityEngine;

namespace SelectionArena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "ScriptableObject/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        [TextArea(2, 5)] public string ArenaName;
        public Sprite ArenaVisual;
        public string ArenaSceneName;
    }
}