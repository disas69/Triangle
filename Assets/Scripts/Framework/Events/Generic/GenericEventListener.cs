using UnityEngine;

namespace Framework.Events.Generic
{
    public abstract class GenericEventListener<T> : MonoBehaviour
    {
        public void OnEventFired(T value)
        {
            Callback(value);
        }

        protected abstract void Callback(T value);
        protected abstract void OnEnable();
        protected abstract void OnDisable();
    }
}