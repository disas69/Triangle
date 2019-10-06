using Framework.Input;
using Framework.Utils.Math;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Triangle.Structure
{
    public class InputHandler
    {
        private readonly PlayerController _player;
        private readonly VectorAverager _dragSpeedAverage;
        private readonly float _screenToWorldScaleFactor;

        private bool _isActive;
        private Vector3 _velocity;
        private bool _dragging;
        private Vector2 _lastDragDelta;

        public InputHandler(PlayerController player)
        {
            _isActive = true;
            _player = player;

            _dragSpeedAverage = new VectorAverager(0.1f);
            _screenToWorldScaleFactor = 2f * Camera.main.orthographicSize / Camera.main.pixelHeight;

            InputEventProvider.Instance.PointerDown += OnPointerDown;
            InputEventProvider.Instance.PointerUp += OnPointerUp;
            InputEventProvider.Instance.BeginDrag += OnBeginDrag;
            InputEventProvider.Instance.Drag += OnDrag;
        }

        public void Update()
        {
            _dragSpeedAverage.AddSample(_lastDragDelta);
            _lastDragDelta = DeselerateDragDelta(_dragSpeedAverage.Value);
        }

        public Vector3 GetPosition()
        {
            var newPosition = new Vector3(_player.transform.position.x, _player.transform.position.z, 0f);
            var velocity = _dragSpeedAverage.Value;

            if (velocity.magnitude > 0.01f)
            {
                var worldSpaceDelta = velocity * _player.Settings.MoveSpeed * _screenToWorldScaleFactor;
                newPosition = Vector3.SmoothDamp(newPosition, newPosition + worldSpaceDelta, ref _velocity, _player.Settings.MoveSmoothing);
            }

            return new Vector3(newPosition.x, 0f, newPosition.y);
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
        }

        private Vector2 DeselerateDragDelta(Vector3 velocity)
        {
            var lastDragDelta = _lastDragDelta;
            if (lastDragDelta.x > 0)
            {
                lastDragDelta.x -= velocity.x * _player.Settings.Deceleration;
            }
            else
            {
                lastDragDelta.x = 0;
            }

            if (lastDragDelta.y > 0)
            {
                lastDragDelta.y -= velocity.y * _player.Settings.Deceleration;
            }
            else
            {
                lastDragDelta.y = 0;
            }

            return lastDragDelta;
        }

        private void OnPointerDown(PointerEventData eventData)
        {
        }

        private void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isActive)
            {
                return;
            }

            _dragging = true;
            _dragSpeedAverage.Clear();
        }

        private void OnDrag(PointerEventData eventData)
        {
            if (!_isActive)
            {
                return;
            }

            _lastDragDelta = eventData.delta;
        }

        private void OnPointerUp(PointerEventData eventData)
        {
            if (InputEventProvider.Instance.PointersCount == 0 && _dragging)
            {
                _dragging = false;
            }
        }

        public void Dispose()
        {
            InputEventProvider.Instance.PointerDown -= OnPointerDown;
            InputEventProvider.Instance.PointerUp -= OnPointerUp;
            InputEventProvider.Instance.BeginDrag -= OnBeginDrag;
            InputEventProvider.Instance.Drag -= OnDrag;
        }
    }
}