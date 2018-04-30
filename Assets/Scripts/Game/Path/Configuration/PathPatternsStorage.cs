using System;
using System.Collections.Generic;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Configuration
{
    [CreateAssetMenu(fileName = "PathPatternsStorage", menuName = "PathGeneration/PathPatternsStorage")]
    public class PathPatternsStorage : ScriptableObject
    {
        public Settings DefaultSettings;
        public List<PathPattern> PathPatterns = new List<PathPattern>();

        public PathPattern GetNextPathPattern()
        {
            PathPattern pattern = null;

            if (PathPatterns.Count > 0)
            {
                var index = UnityEngine.Random.Range(0, PathPatterns.Count);
                pattern = PathPatterns[index];
            }

            return pattern;
        }
    }

    [Serializable]
    public class PathPattern
    {
        private int _index = 0;

        public string Name;
        public List<Settings> Values = new List<Settings>();

        public Settings GetNextValue()
        {
            Settings value = null;

            if (_index < Values.Count)
            {
                value = Values[_index];
                _index++;
            }
            else
            {
                _index = 0;
            }

            return value;
        }
    }

    [Serializable]
    public class Settings
    {
        public LineType LineType;
        public bool Countable = true;
        public float Length = 20f;
        public float RotationAngle;
    }
}
