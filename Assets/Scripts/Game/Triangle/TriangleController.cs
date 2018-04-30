using System;
using System.Collections;
using Framework.Extensions;
using Game.Input.Structure;
using Game.Path.Lines.Base;
using Game.Triangle.Configuration;
using Game.Triangle.Structure;
using Game.Triangle.Structure.Interface;
using UnityEngine;

namespace Game.Triangle
{
    [RequireComponent(typeof(PathProgressHandler))]
    public class TriangleController : MonoBehaviour
    {
        private bool _isActive;
        private Vector3 _velocity;
        private Vector3 _initialScale;
        private Vector3 _targetPosition;
        private Coroutine _rotationCoroutine;
        private Coroutine _scaleChangeCoroutine;

        private IInputHandler _inputHandler;
        private ILineTouchHandler _lineTouchHandler;
        private IPathProgressHandler _pathProgressHandler;

        [SerializeField] private TriangleSettings _triangleSettings;

        public Vector3 DragDirection
        {
            get { return _inputHandler.DragDirection; }
        }

        public event Action LinePassed;
        public event Action LineTouched;

        public void SetupInput(IInputProvider inputProvider)
        {
            _inputHandler = new InputHandler(this, inputProvider);
        }

        private void Start()
        {
            _isActive = true;
            _initialScale = transform.localScale;

            _lineTouchHandler = new LineTouchHandler(this);
            _lineTouchHandler.LineTouched += OnLineTouched;

            _pathProgressHandler = GetComponent<PathProgressHandler>();
            _pathProgressHandler.LineTriggered += OnLineTriggered;
            _pathProgressHandler.LinePassed += OnLinePassed;
        }

        private void Update()
        {
            if (_inputHandler.IsActive)
            {
                _targetPosition = _inputHandler.GetWorldPosition();
            }

            if (_isActive)
            {
                var newPosition = Vector3.SmoothDamp(transform.position, _targetPosition,
                    ref _velocity, _triangleSettings.MoveSmoothing);

                transform.position = newPosition;

                RotateAround();
            }
        }

        private void RotateAround()
        {
            transform.Rotate(Vector3.back, _triangleSettings.RotationSpeed.Value * Time.deltaTime);
        }

        private void OnLineTriggered(Line pathLine)
        {
            if (_isActive)
            {
                this.SafeStopCoroutine(_rotationCoroutine);
                _rotationCoroutine = StartCoroutine(RotateToLine(pathLine.transform.rotation));
            }
        }

        private void OnLinePassed(Line pathLine)
        {
            if (_isActive)
            {
                this.SafeStopCoroutine(_scaleChangeCoroutine);
                _scaleChangeCoroutine = StartCoroutine(ChangeScale(_triangleSettings.ScaleMultiplier));

                LinePassed.SafeInvoke();
            }
        }

        private IEnumerator RotateToLine(Quaternion lineRotation)
        {
            var time = 0f;
            while (time < _triangleSettings.RotateToLineTime)
            {
                var eulerAngles = lineRotation.eulerAngles;
                eulerAngles.x = transform.localEulerAngles.x;
                eulerAngles.z = transform.localEulerAngles.z;
                lineRotation.eulerAngles = eulerAngles;

                var newRotation = Quaternion.Lerp(transform.rotation, lineRotation,
                    time / _triangleSettings.RotateToLineTime);
                transform.rotation = newRotation;
                time += Time.deltaTime;

                yield return null;
            }

            transform.rotation = lineRotation;
            _rotationCoroutine = null;
        }

        private IEnumerator ChangeScale(float multiplier)
        {
            transform.localScale = _initialScale * multiplier;

            var time = 0f;
            while (time < _triangleSettings.ScaleChangeTime)
            {
                var scale = Vector3.Lerp(transform.localScale, _initialScale,
                    time / _triangleSettings.ScaleChangeTime);
                transform.localScale = scale;
                time += Time.deltaTime;

                yield return null;
            }

            transform.localScale = _initialScale;
            _scaleChangeCoroutine = null;
        }

        private void OnLineTouched()
        {
            _isActive = false;
            _inputHandler.Deactivate();

            this.SafeStopCoroutine(_rotationCoroutine);
            this.SafeStopCoroutine(_scaleChangeCoroutine);

            LineTouched.SafeInvoke();
        }

        private void OnDestroy()
        {
            _inputHandler.Dispose();

            _lineTouchHandler.LineTouched -= OnLineTouched;
            _lineTouchHandler.Dispose();

            _pathProgressHandler.LineTriggered -= OnLineTriggered;
            _pathProgressHandler.LinePassed -= OnLinePassed;
        }
    }
}