using System;
using Framework.Events.Generic;
using UnityEngine;

namespace Framework.Events.String
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringEvent", menuName = "Events/StringEvent")]
    public class StringEvent : GenericEvent<string>
    {
    }
}