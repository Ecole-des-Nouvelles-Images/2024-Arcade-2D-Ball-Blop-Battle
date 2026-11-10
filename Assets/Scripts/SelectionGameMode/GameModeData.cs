using Managers;
using UnityEngine;

namespace SelectionGameMode
{
    [CreateAssetMenu(fileName = "GameModeData", menuName = "ScriptableObject/GameModeData")]
    public class GameModeData : ScriptableObject
    {
        public GameMode GameModeType;
        [TextArea(2, 5)] public string SceneName;
        [TextArea(2, 5)] public string Text;
        public Sprite FirstOponent;
        public Sprite SecondOponent;
    }
}