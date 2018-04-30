using Game.Input.Structure;
using Game.Triangle.Structure.Interface;
using UnityEngine;

namespace Game.Triangle.Structure
{
    public class InputHandler : IInputHandler
    {
        private readonly TriangleController _triangleController;
        private readonly IInputProvider _inputProvider;
        private readonly int _inputReceiverMask;
        private readonly Camera _camera;

        private Vector3 _touchOffset;
        private Vector3 _lastTouchPosition;

        public bool IsActive { get; private set; }
        public Vector3 DragDirection { get; private set; }

        public InputHandler(TriangleController triangleController, IInputProvider inputProvider)
        {
            _triangleController = triangleController;
            _inputProvider = inputProvider;
            _inputReceiverMask = LayerMask.GetMask("InputReceiver");
            _camera = Camera.main;

            _inputProvider.PointerDown += OnPointerDown;
            _inputProvider.PointerUp += OnPointerUp;
        }

        public Vector3 GetWorldPosition()
        {
            var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, _inputReceiverMask))
            {
                var touchPosition = hit.point;
                touchPosition.y = 0f;

                DragDirection = (touchPosition - _lastTouchPosition).normalized;
                _lastTouchPosition = touchPosition;

                return touchPosition + _touchOffset;
            }

            return Vector3.zero;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void Dispose()
        {
            _inputProvider.PointerDown -= OnPointerDown;
            _inputProvider.PointerUp -= OnPointerUp;
        }

        private void OnPointerDown(Vector3 position)
        {
            var ray = _camera.ScreenPointToRay(position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f, _inputReceiverMask))
            {
                IsActive = true;

                var touchPosition = hit.point;
                touchPosition.y = 0f;

                _touchOffset = _triangleController.transform.position - touchPosition;
                _lastTouchPosition = touchPosition;
            }
        }

        private void OnPointerUp(Vector3 position)
        {
            IsActive = false;
        }
    }
}