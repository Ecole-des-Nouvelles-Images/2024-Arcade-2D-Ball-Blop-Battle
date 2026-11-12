using Player.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SelectionPlayer
{
    /// <summary>
    /// État d'un emplacement joueur (P1 / P2) dans l'écran de sélection.
    /// Un emplacement sans device est une IA s'il peut l'être, sinon il attend qu'un joueur le rejoigne.
    /// </summary>
    public class SelectionSlot
    {
        public int PlayerId { get; }
        public bool CanBeAi { get; }

        public SelectionInput Input { get; private set; }
        public int CursorIndex { get; set; }
        public Blop ConfirmedBlop { get; private set; }

        private int _joinFrame = -1;

        public SelectionSlot(int playerId, bool canBeAi, int cursorIndex)
        {
            PlayerId = playerId;
            CanBeAi = canBeAi;
            CursorIndex = cursorIndex;
        }

        public bool HasDevice => Input != null;
        public bool IsAi => CanBeAi && !HasDevice;
        public bool IsWaitingForPlayer => !CanBeAi && !HasDevice;
        public bool IsConfirmed => ConfirmedBlop != null;

        /// <summary>Vrai pendant la frame où le joueur a rejoint (son appui sur "valider" ne doit pas confirmer).</summary>
        public bool JoinedThisFrame => _joinFrame == Time.frameCount;

        public void AssignInput(SelectionInput input)
        {
            Input?.Dispose();
            Input = input;
            _joinFrame = Time.frameCount;
        }

        public void RemoveInput()
        {
            Input?.Dispose();
            Input = null;
        }

        public bool Owns(InputDevice device) => Input != null && Input.Device == device;

        public void Confirm(Blop blop) => ConfirmedBlop = blop;

        public void Unconfirm() => ConfirmedBlop = null;
    }
}
