using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Events.Generic
{
    [Serializable]
    public abstract class GenericEvent<T> : ScriptableObject
    {
        private readonly List<GenericEventListener<T>> _eventListeners = new List<GenericEventListener<T>>();

        public void Register(GenericEventListener<T> listener)
        {
            if (!_eventListeners.Contains(listener))
            {
                _eventListeners.Add(listener);
            }
        }

        public void Unregister(GenericEventListener<T> listener)
        {
            _eventListeners.Remove(listener);
        }

        public void Fire(T value)
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
            {
                _eventListeners[i].OnEventFired(value);
            }
        }
    }
}