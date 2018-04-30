using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Structure
{
    public class GameSnapshot
    {
        private readonly Transform _root;
        private readonly List<Vector3> _initialPositions;

        public GameSnapshot(Transform root)
        {
            _root = root;
            _initialPositions = new List<Vector3>(_root.childCount);
        }

        public void SaveState()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                _initialPositions.Add(child.transform.position);
            }
        }

        public void RestoreState()
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                var child = _root.GetChild(i);
                child.transform.position = _initialPositions[i];
            }
        }
    }
}