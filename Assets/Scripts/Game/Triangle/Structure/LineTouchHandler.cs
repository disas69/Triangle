using System;
using Framework.Extensions;
using Game.Triangle.Structure.Interface;

namespace Game.Triangle.Structure
{
    public class LineTouchHandler : ILineTouchHandler
    {
        private readonly TriangleController _triangleController;
        private readonly TouchablePiece[] _pieces;

        public event Action LineTouched;

        public LineTouchHandler(TriangleController triangleController)
        {
            _triangleController = triangleController;
            _pieces = _triangleController.GetComponentsInChildren<TouchablePiece>();

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
                _pieces[i].AddForce(_triangleController.DragDirection);
            }
        }
    }
}