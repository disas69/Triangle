using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Lines.Configuration
{
    [CreateAssetMenu(fileName = "DancingPathLineSettings", menuName = "PathLines/DancingPathLineSettings")]
    public class DancingPathLineSettings : PathLineSettings
    {
        public float MinScaledTime;
        public float MaxScaledTime;
        public float MinScaleMultiplier;
        public float MaxScaleMultiplier;
    }
}