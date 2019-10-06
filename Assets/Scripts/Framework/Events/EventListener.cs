using UnityEngine;
using UnityEngine.Events;

namespace Framework.Events
{
    public class EventListener : MonoBehaviour
    {
        public Event Event;
        public UnityEvent Response;

        public void OnEventFired()
        {
            Response.Invoke();
        }

        private void OnEnable()
        {
            Event.Register(this);
        }

        private void OnDisable()
        {
            Event.Unregister(this);
        }
    }
}