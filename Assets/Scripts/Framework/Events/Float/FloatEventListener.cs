using Framework.Events.Generic;
using UnityEngine;

namespace Framework.Events.Float
{
    public class FloatEventListener : GenericEventListener<float>
    {
        [SerializeField] private FloatEvent _event;
        [SerializeField] private UnityFloatEvent _response;
        
        protected override void Callback(float value)
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