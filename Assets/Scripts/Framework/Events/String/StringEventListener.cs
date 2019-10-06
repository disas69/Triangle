using Framework.Events.Generic;
using Framework.UnityEvents;
using UnityEngine;

namespace Framework.Events.String
{
    public class StringEventListener : GenericEventListener<string>
    {
        [SerializeField] private StringEvent _event;
        [SerializeField] private UnityStringEvent _response;

        protected override void Callback(string value)
        {
            _response.Invoke(value);
        }

        protected override void OnEnable()
        {
            _event.Register(this);
        }

        protected override void OnDisable()
        {
            _event.Unregister(this);
        }
    }
}