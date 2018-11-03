using System;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Utils.Physics
{
    [RequireComponent(typeof(Collider))]
    public abstract class ContactHandler<T> : MonoBehaviour where T : Component
    {
        public event Action<T> Entered;
        public event Action<T> Exited;

        private void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<T>();
            if (component != null)
            {
                Entered.SafeInvoke(component);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var component = other.GetComponent<T>();
            if (component != null)
            {
                Exited.SafeInvoke(component);
            }
        }
    }
}