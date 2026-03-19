using System;
using Player.ScriptableObjects;

namespace Utils
{
    public static class EventBus
    {
        // GAME
        public static Action OnGameLaunched;
        public static Action OnGamePaused;
        public static Action OnGameResumed;
        
        // MATCH
        public static Action<int> OnPlayerScored;
        public static Action<int> OnSetChangement;
        public static Action OnSetIsOver;
        public static Action OnFoul;
        public static Action<int> OnMatchOver;
        
        // PLAYER
        public static Action<int> OnPlayerTouchedBall;
        public static Action<int ,int> OnPlayerPerfectReception;
        public static Action<int, BlopType> OnSpecialSpikeActivated;
        public static Action<int, BlopType> OnAbsorbedSpecialSpike;
        public static Action<int> OnPlayerDie;
        
        // BALL
        public static Action OnPlayerCommitment;
    }
}