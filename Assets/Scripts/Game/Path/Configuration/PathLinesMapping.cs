using System;
using System.Collections.Generic;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Path.Configuration
{
    [CreateAssetMenu(fileName = "PathLinesMapping", menuName = "PathGeneration/PathLinesMapping")]
    public class PathLinesMapping : ScriptableObject
    {
        private Dictionary<LineType, Line> _linesDictionary;

        public List<PathLinePrefab> PathLinePrefabs = new List<PathLinePrefab>();

        public Line GetLinePrefab(LineType lineType)
        {
            if (_linesDictionary == null)
            {
                InitializeDictionary();
            }

            Line pathPathLine;
            if (_linesDictionary.TryGetValue(lineType, out pathPathLine))
            {
                return pathPathLine;
            }

            Debug.LogErrorFormat("Failed to find line prefab associated with type: {0}", lineType);
            return null;
        }

        private void InitializeDictionary()
        {
            _linesDictionary = new Dictionary<LineType, Line>(PathLinePrefabs.Count);

            for (int i = 0; i < PathLinePrefabs.Count; i++)
            {
                var pathLine = PathLinePrefabs[i];
                _linesDictionary.Add(pathLine.Type, pathLine.Prefab);
            }
        }
    }

    [Serializable]
    public class PathLinePrefab
    {
        public LineType Type;
        public Line Prefab;
    }
}