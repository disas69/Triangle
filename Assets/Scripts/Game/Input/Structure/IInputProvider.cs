using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input.Structure
{
    public interface IInputProvider : IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        event Action<Vector3> PointerDown;
        event Action<Vector3> PointerUp;
        event Action<Vector3> Drag;
    }
}