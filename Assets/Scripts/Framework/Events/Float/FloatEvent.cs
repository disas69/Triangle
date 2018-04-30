using System;
using Framework.Events.Generic;
using UnityEngine;

namespace Framework.Events.Float
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Events/FloatEvent")]
    public class FloatEvent : GenericEvent<float>
    {
    }
}