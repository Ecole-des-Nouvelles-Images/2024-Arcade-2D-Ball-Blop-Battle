using System;

namespace Int.Scripts.Utils
{
    public static class EventBus
    {
        // MATCH
        public static Action<int> OnPlayerScored;
        public static Action<int> OnSetChangement;
        public static Action OnSetIsOver;
        
        // BALL
        public static Action OnPlayerCommitment;
    }
}
