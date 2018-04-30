using Framework.Variables;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Lines.Configuration
{
    [CreateAssetMenu(fileName = "FastPathLineSettings", menuName = "PathLines/FastPathLineSettings")]
    public class FastPathLineSettings : PathLineSettings
    {
        public float SpeedIncreaseMultiplier;
        public FloatVariable PathSpeedMultiplier;
        public FloatVariable TriangleRotationSpeed;
    }
}