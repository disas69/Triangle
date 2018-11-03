using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Structure
{
    public class SceneSnapshot
    {
        private readonly Transform _root;
        private readonly List<Vector3> _initialPositions;

        public SceneSnapshot(Transform root)
        {
            _root = root;
            _initialPositions = new List<Vector3>(_root.childCount);
        }

        public void Save()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                _initialPositions.Add(child.transform.position);
            }
        }

        public void Restore()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                child.transform.position = _initialPositions[i];
            }
        }
    }
}