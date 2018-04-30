using System;
using Framework.Extensions;
using Game.Input.Structure;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class InputProvider : MonoBehaviour, IInputProvider
    {
        public event Action<Vector3> PointerDown;
        public event Action<Vector3> PointerUp;
        public event Action<Vector3> Drag;

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown.SafeInvoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp.SafeInvoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag.SafeInvoke(eventData.position);
        }
    }
}