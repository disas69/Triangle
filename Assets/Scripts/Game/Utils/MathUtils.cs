using UnityEngine;

namespace Assets.Scripts.Game
{
    public static class MathUtils
    {
        public static float GetDistanceToSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var distanceToA = Mathf.Sqrt((segmentStart.x - point.x) * (segmentStart.x - point.x) +
                                         (segmentStart.y - point.y) * (segmentStart.y - point.y));
            var distanceToB = Mathf.Sqrt((segmentEnd.x - point.x) * (segmentEnd.x - point.x) +
                                         (segmentEnd.y - point.y) * (segmentEnd.y - point.y));

            if (((point.x > segmentStart.x || point.x > segmentEnd.x) &&
                 (point.x < segmentEnd.x || point.x < segmentStart.x)) ||
                ((point.y > segmentStart.y || point.y > segmentEnd.y) &&
                 (point.y < segmentEnd.y || point.y < segmentStart.y)))
            {
                return Mathf.Abs((segmentEnd.y - segmentStart.y) * point.x - (segmentEnd.x - segmentStart.x) * point.y +
                                 segmentEnd.x * segmentStart.y - segmentEnd.y * segmentStart.x) /
                       Mathf.Sqrt((segmentEnd.y - segmentStart.y) * (segmentEnd.y - segmentStart.y) +
                                  (segmentEnd.x - segmentStart.x) * (segmentEnd.x - segmentStart.x));
            }

            return Mathf.Min(distanceToA, distanceToB);
        }
    }
}