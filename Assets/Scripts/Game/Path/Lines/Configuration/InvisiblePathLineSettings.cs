using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Lines.Configuration
{
    [CreateAssetMenu(fileName = "InvisiblePathLineSettings", menuName = "PathLines/InvisiblePathLineSettings")]
    public class InvisiblePathLineSettings : PathLineSettings
    {
        public float MinInvisibleTime;
        public float MaxInvisibleTime;
    }
}