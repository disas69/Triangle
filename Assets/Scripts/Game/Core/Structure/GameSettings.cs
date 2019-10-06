using UnityEngine;

namespace Game.Core.Structure
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings")]
    public class GameSettings : ScriptableObject
    {
        public float ActivationTime;
    }
}