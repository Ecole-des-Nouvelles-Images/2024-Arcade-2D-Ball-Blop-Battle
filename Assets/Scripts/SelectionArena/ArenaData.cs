using UnityEngine;
using UnityEngine.Localization;

namespace SelectionArena
{
    [CreateAssetMenu(fileName = "ArenaData", menuName = "ScriptableObject/ArenaData")]
    public class ArenaData : ScriptableObject
    {
        public LocalizedString LocalizedArenaName;
        public string ArenaName;
        public Sprite ArenaVisual;
        public string ArenaSceneName;
        
        public string GetArenaName()
        {
            return LocalizedArenaName != null && !LocalizedArenaName.IsEmpty
                ? LocalizedArenaName.GetLocalizedString()
                : ArenaName;
        }
    }
}