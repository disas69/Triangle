using System;
using Framework.Extensions;
using Game.Path.Lines.Base;
using Game.Triangle.Structure.Interface;
using UnityEngine;

namespace Game.Triangle.Structure
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
    public class PathProgressHandler : MonoBehaviour, IPathProgressHandler
    {
        private Line _currentPathPathLine;

        public event Action<Line> LineTriggered;
        public event Action<Line> LinePassed;

        private void Update()
        {
            if (_currentPathPathLine != null)
            {
                _currentPathPathLine.SetProgress(transform.position);
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            var pathLine = otherCollider.GetComponent<Line>();
            if (pathLine != null && !pathLine.IsTriggered)
            {
                pathLine.IsTriggered = true;
                _currentPathPathLine = pathLine;

                if (pathLine.Countable)
                {
                    LineTriggered.SafeInvoke(pathLine);
                }
            }
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            var pathLine = otherCollider.GetComponent<Line>();
            if (pathLine != null && !pathLine.IsPassed)
            {
                pathLine.IsPassed = true;

                if (pathLine.Countable)
                {
                    LinePassed.SafeInvoke(pathLine);
                }
            }
        }
    }
}