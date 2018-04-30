using Framework.Variables;
using UnityEngine;

namespace Game.Path.Configuration
{
    [CreateAssetMenu(fileName = "PathSettings", menuName = "PathGeneration/PathSettings")]
    public class PathSettings : ScriptableObject
    {
        public int InitialPathSize;
        public int MaxPathSize;
        public int PoolSize;
        public float InitialMoveSpeed;
        public float MaxMoveSpeed;
        public FloatVariable SpeedMultiplier;
    }
}