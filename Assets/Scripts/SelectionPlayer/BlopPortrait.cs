using Player.ScriptableObjects;
using UnityEngine;

namespace SelectionPlayer
{
    /// <summary>Case de la grille de sélection, associée à un Blop.</summary>
    [RequireComponent(typeof(RectTransform))]
    public class BlopPortrait : MonoBehaviour
    {
        [Header("===== DATA =====")]
        [SerializeField] private Blop _blop;

        public Blop Blop => _blop;
        public RectTransform RectTransform => (RectTransform) transform;
    }
}
