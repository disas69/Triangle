using UnityEngine;

namespace Game.Path.Lines.Base
{
    public abstract class PathLine<T> : Line where T : PathLineSettings
    {
        [SerializeField] private T _settings;

        public T Settings
        {
            get { return _settings; }
        }
    }
}