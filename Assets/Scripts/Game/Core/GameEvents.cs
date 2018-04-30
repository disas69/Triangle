using System;
using Framework.Events;

namespace Game.Core
{
    [Serializable]
    public class GameEvents
    {
        public Event Passed10LinesEvent;
        public Event BestScoreBeatenEvent;
    }
}