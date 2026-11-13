using System;

namespace Bots
{
    /// <summary>
    /// Limites physiques de l'arène utilisées par la prédiction de balle.
    /// Les valeurs par défaut correspondent au prefab Arene_Colliders (faces internes des colliders).
    /// </summary>
    [Serializable]
    public struct ArenaBounds
    {
        public float LeftWallX;
        public float RightWallX;
        public float CeilingY;
        public float GroundY;

        public float NetX;
        public float NetHalfWidth;
        public float NetTopY;

        public float Bounciness;

        public static ArenaBounds Default => new ArenaBounds
        {
            LeftWallX = -8.45f,
            RightWallX = 8.47f,
            CeilingY = 3.53f,
            GroundY = -4.45f,
            NetX = 0f,
            NetHalfWidth = 0.15f,
            NetTopY = -1.0f,
            Bounciness = 0.8f
        };
    }
}
