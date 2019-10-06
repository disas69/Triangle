using System;
using Framework.Events.Generic;
using UnityEngine;

namespace Framework.Events.Int
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Events/IntEvent")]
    public class IntEvent : GenericEvent<int>
    {
    }
}