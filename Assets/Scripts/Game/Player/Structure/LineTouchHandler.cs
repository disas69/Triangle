using System;
using Framework.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Triangle.Structure
{
    public class LineTouchHandler
    {
        private readonly TouchablePiece[] _pieces;

        public event Action LineTouched;

        public LineTouchHandler(TouchablePiece[] pieces)
        {
            _pieces = pieces;

            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].LineTouched += OnPathLineTouched;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].LineTouched -= OnPathLineTouched;
            }
        }

        private void OnPathLineTouched()
        {
            Dispose();
            AddForceToPieces();
            LineTouched.SafeInvoke();
        }

        private void AddForceToPieces()
        {
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            }
        }
    }
}