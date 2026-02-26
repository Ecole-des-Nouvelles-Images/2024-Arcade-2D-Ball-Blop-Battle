using System;

namespace Int.Scripts.Utils
{
    public static class EventBus
    {
        // MATCH
        public static Action<int> OnPlayerScored;
        public static Action<int> OnSetChangement;
        public static Action OnSetIsOver;
        
        // PLAYER
        public static Action OnSpecialSpikeActivated;
        
        // BALL
        public static Action OnPlayerCommitment;
    }
}
