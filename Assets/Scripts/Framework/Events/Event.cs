using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Events
{
    [Serializable]
    [CreateAssetMenu(fileName = "Event", menuName = "Events/Event")]
    public class Event : ScriptableObject
    {
        private readonly List<EventListener> _eventListeners = new List<EventListener>();

        public void Register(EventListener listener)
        {
            if (!_eventListeners.Contains(listener))
            {
                _eventListeners.Add(listener);
            }
        }

        public void Unregister(EventListener listener)
        {
            _eventListeners.Remove(listener);
        }

        public void Fire()
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
            {
                _eventListeners[i].OnEventFired();
            }
        }
    }
}