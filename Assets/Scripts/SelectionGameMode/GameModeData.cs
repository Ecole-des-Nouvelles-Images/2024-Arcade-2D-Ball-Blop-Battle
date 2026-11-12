using Managers;
using UnityEngine;

namespace SelectionGameMode
{
    /// <summary>
    /// Mode de jeu et scène de gameplay associée.
    /// Choisi automatiquement par l'écran de sélection : PvE si le J2 est une IA, PvP sinon.
    /// </summary>
    [CreateAssetMenu(fileName = "GameModeData", menuName = "ScriptableObject/GameModeData")]
    public class GameModeData : ScriptableObject
    {
        public GameMode GameModeType;
        public string SceneName;
    }
}
