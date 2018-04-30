using System;
using UnityEngine;

namespace Game.Triangle.Structure.Interface
{
    public interface IInputHandler : IDisposable
    {
        bool IsActive { get; }
        Vector3 DragDirection { get; }
        Vector3 GetWorldPosition();
        void Deactivate();
    }
}