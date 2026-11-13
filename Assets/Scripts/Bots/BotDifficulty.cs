using UnityEngine;
using UnityEngine.Localization;

namespace Bots
{
    /// <summary>
    /// Réglages de difficulté du bot. La FSM ne change pas entre les niveaux :
    /// seuls la perception, la précision et les choix varient.
    /// </summary>
    [CreateAssetMenu(fileName = "BotDifficulty", menuName = "Bots/Bot Difficulty")]
    public class BotDifficulty : ScriptableObject
    {
        [Header("===== AFFICHAGE =====")]
        [Tooltip("Nom localisé affiché dans l'écran de sélection.")]
        public LocalizedString LocalizedName;

        [Tooltip("Nom de secours si LocalizedName n'est pas renseigné.")]
        public string DisplayName = "Moyen";

        [Tooltip("Position dans la liste de sélection (0 = la plus facile).")]
        public int Order = 1;

        [Header("===== PERCEPTION =====")]
        [Tooltip("Retard (s) entre l'état réel de la balle et ce que le bot perçoit.")]
        [Range(0f, 0.5f)] public float ReactionTime = 0.15f;

        [Tooltip("Intervalle (s) entre deux décisions.")]
        [Range(0.02f, 0.2f)] public float DecisionInterval = 0.04f;

        [Tooltip("Erreur max (unités) sur la position visée, retirée à chaque touche de balle.")]
        [Range(0f, 1.5f)] public float PositioningError = 0.3f;

        [Header("===== DÉCISIONS =====")]
        [Tooltip("Probabilité de tenter une réception parfaite quand elle est possible.")]
        [Range(0f, 1f)] public float PerfectReceptionChance = 0.6f;

        [Tooltip("Le bot peut-il dasher pour rattraper une balle lointaine ?")]
        public bool UseDash = true;

        [Header("===== VISÉE =====")]
        [Tooltip("0 = choisit au hasard parmi les tirs valides, 1 = choisit toujours le meilleur.")]
        [Range(0f, 1f)] public float AimSkill = 0.7f;

        [Tooltip("Erreur angulaire max (degrés) appliquée au tir choisi.")]
        [Range(0f, 25f)] public float AimErrorDegrees = 6f;

        /// <summary>Nom dans la langue courante, ou <see cref="DisplayName"/> à défaut.</summary>
        public string GetDisplayName()
        {
            return LocalizedName != null && !LocalizedName.IsEmpty ? LocalizedName.GetLocalizedString() : DisplayName;
        }
    }
}
