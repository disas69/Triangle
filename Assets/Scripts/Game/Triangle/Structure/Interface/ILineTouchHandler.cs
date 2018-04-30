using System;

namespace Game.Triangle.Structure.Interface
{
    public interface ILineTouchHandler : IDisposable
    {
        event Action LineTouched;
    }
}