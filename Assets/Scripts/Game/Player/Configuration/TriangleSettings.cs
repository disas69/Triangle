using Framework.Variables;
using UnityEngine;

namespace Game.Triangle.Configuration
{
    [CreateAssetMenu(fileName = "TriangleSettings", menuName = "Triangle/TriangleSettings")]
    public class TriangleSettings : ScriptableObject
    {
        public FloatVariable RotationSpeed;
        public float RotateToLineTime;
        public float ScaleChangeTime;
        public float ScaleMultiplier;
        public float MoveSpeed;
        public float FixSpeed;
        public float Deceleration;
        public float MoveSmoothing;
        public float FixSmoothing;
    }
}
