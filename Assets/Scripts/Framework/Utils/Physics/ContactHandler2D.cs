using System;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Utils.Physics
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ContactHandler2D<T> : MonoBehaviour where T : Component
    {
        public event Action<T> Entered;
        public event Action<T> Exited;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var component = other.GetComponent<T>();
            if (component != null)
            {
                Entered.SafeInvoke(component);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var component = other.GetComponent<T>();
            if (component != null)
            {
                Exited.SafeInvoke(component);
            }
        }
    }
}