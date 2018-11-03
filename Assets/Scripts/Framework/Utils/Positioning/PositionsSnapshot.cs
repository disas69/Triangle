using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utils.Positioning
{
    public class PositionsSnapshot
    {
        private readonly Transform _root;
        private readonly List<Vector3> _initialPositions;

        public PositionsSnapshot(Transform root)
        {
            _root = root;
            _initialPositions = new List<Vector3>(_root.childCount);
        }

        public void SavePositions()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                _initialPositions.Add(child.transform.position);
            }
        }

        public void RestorePositions()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                child.transform.position = _initialPositions[i];
            }
        }
    }
}