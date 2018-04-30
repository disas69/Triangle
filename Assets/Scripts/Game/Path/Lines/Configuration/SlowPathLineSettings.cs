using Framework.Variables;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Lines.Configuration
{
    [CreateAssetMenu(fileName = "SlowPathLineSettings", menuName = "PathLines/SlowPathLineSettings")]
    public class SlowPathLineSettings : PathLineSettings
    {
        public float SpeedDecreaseMultiplier;
        public FloatVariable PathSpeedMultiplier;
        public FloatVariable TriangleRotationSpeed;
    }
}