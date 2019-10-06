using System;
using Framework.Extensions;
using Game.Path.Lines.Base;
using UnityEngine;

namespace Game.Triangle.Structure
{
    [RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(BoxCollider))]
    public class PathProgressHandler : MonoBehaviour
    {
        private Line _pathLine;

        public event Action<Line> LineTriggered;
        public event Action<Line> LinePassed;

        private void Update()
        {
            if (_pathLine != null)
            {
                if (_pathLine.IsTriggered && !_pathLine.IsPassed)
                {
                    _pathLine.SetProgress(transform.position);
                }
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            var pathLine = otherCollider.GetComponent<Line>();
            if (pathLine != null && !pathLine.IsTriggered)
            {
                pathLine.IsTriggered = true;
                LineTriggered.SafeInvoke(pathLine);

                if (_pathLine != null)
                {
                    _pathLine.ChangeMaterial();
                }

                _pathLine = pathLine;
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