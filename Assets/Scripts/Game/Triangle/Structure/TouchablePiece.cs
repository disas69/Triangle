using System;
using Framework.Extensions;
using Game.Path.Lines.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Triangle.Structure
{
    public class TouchablePiece : MonoBehaviour
    {
        private readonly Vector3[] _forces =
        {
            Vector3.up, Vector3.right, Vector3.left, Vector3.down
        };

        private Rigidbody _rigidbody;

        public event Action LineTouched;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            var line = otherCollider.GetComponent<Line>();
            if (line != null)
            {
                LineTouched.SafeInvoke();
            }
        }

        public void AddForce(Vector3 force)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;

            if (force == Vector3.zero)
            {
                force = Vector3.forward;
            }

            var forceMultiplier = Random.Range(10f, 20f);
            _rigidbody.AddForce(force * forceMultiplier, ForceMode.Impulse);
            _rigidbody.AddTorque(_forces[Random.Range(0, _forces.Length)] * forceMultiplier, ForceMode.Impulse);
        }
    }
}