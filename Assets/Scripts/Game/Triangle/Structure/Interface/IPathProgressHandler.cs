using System;
using Game.Path.Lines.Base;

namespace Game.Triangle.Structure.Interface
{
    public interface IPathProgressHandler
    {
        event Action<Line> LineTriggered;
        event Action<Line> LinePassed;
    }
}