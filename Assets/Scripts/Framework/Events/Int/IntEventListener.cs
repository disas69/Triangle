using Framework.Events.Generic;
using Framework.UnityEvents;
using UnityEngine;

namespace Framework.Events.Int
{
    public class IntEventListener : GenericEventListener<int>
    {
        [SerializeField] private IntEvent _event;
        [SerializeField] private UnityIntEvent _response;

        protected override void Callback(int value)
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