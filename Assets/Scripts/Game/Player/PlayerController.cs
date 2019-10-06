using System;
using System.Collections;
using Assets.Scripts.Game;
using DG.Tweening;
using Framework.Extensions;
using Framework.Input;
using Game.Core;
using Game.Path.Lines.Base;
using Game.Triangle.Configuration;
using Game.Triangle.Structure;
using UnityEngine;

namespace Game.Triangle
{
    [RequireComponent(typeof(PathProgressHandler))]
    public class PlayerController : MonoBehaviour
    {
        private bool _isActive;
        private Vector3 _velocity;
        private Vector3 _initialScale;
        private Coroutine _rotationCoroutine;
        private Coroutine _scaleChangeCoroutine;
        private Line _currentLine;

        private InputHandler _inputHandler;
        private LineTouchHandler _lineTouchHandler;
        private PathProgressHandler _pathProgressHandler;

        [SerializeField] private TriangleSettings _triangleSettings;

        public TriangleSettings Settings => _triangleSettings;

        public event Action LinePassed;
        public event Action LineTouched;

        private void Start()
        {
            _initialScale = transform.localScale;

            _inputHandler = new InputHandler(this);
            _lineTouchHandler = new LineTouchHandler(GetComponentsInChildren<TouchablePiece>());
            _lineTouchHandler.LineTouched += OnLineTouched;

            _pathProgressHandler = GetComponent<PathProgressHandler>();
            _pathProgressHandler.LineTriggered += OnLineTriggered;
            _pathProgressHandler.LinePassed += OnLinePassed;
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
            _inputHandler.Activate(isActive);
        }

        private void Update()
        {
            _inputHandler.Update();

            if (_isActive)
            {
                var position = _inputHandler.GetPosition();

                //var currentLine = GameController.Instance.Path.GetLineInPosition(transform.position);
                if (_currentLine != null)
                {
                    var fixedPosition = GetFixedPosition(transform.position, _currentLine);
                    position = Vector3.SmoothDamp(position, fixedPosition, ref _velocity, _triangleSettings.FixSmoothing, _triangleSettings.FixSpeed);
                }

                position.y = 0f;
                transform.position = position;
            }

            RotateAround();
        }

        private Vector3 GetFixedPosition(Vector3 position, Line line)
        {
            var a = Vector2.Distance(new Vector2(position.x, position.z),
                new Vector2(line.StartPointPosition.x, line.StartPointPosition.z));
            var b = MathUtils.GetDistanceToSegment(new Vector2(position.x, position.z),
                new Vector2(line.StartPointPosition.x, line.StartPointPosition.z),
                new Vector2(line.EndPointPosition.x, line.EndPointPosition.z));

            if (b > 0)
            {
                var c = Mathf.Sqrt(a * a + b * b);
                return Vector2.MoveTowards(new Vector2(line.StartPointPosition.x, line.StartPointPosition.z),
                    new Vector2(line.EndPointPosition.x, line.EndPointPosition.z), c);
            }

            return position;
        }

        private void RotateAround()
        {
            transform.Rotate(Vector3.back, _triangleSettings.RotationSpeed.Value * Time.deltaTime);
        }

        private void OnLineTriggered(Line pathLine)
        {
            _currentLine = pathLine;

            if (_isActive)
            {
                this.SafeStopCoroutine(_rotationCoroutine);
                _rotationCoroutine = StartCoroutine(RotateToLine(pathLine.transform.rotation));
                //transform.DORotateQuaternion(pathLine.transform.rotation, _triangleSettings.RotateToLineTime);
            }
        }

        private void OnLinePassed(Line pathLine)
        {
            if (_isActive)
            {
                this.SafeStopCoroutine(_scaleChangeCoroutine);
                _scaleChangeCoroutine = StartCoroutine(ChangeScale(_triangleSettings.ScaleMultiplier));
                //transform.localScale = _initialScale * _triangleSettings.ScaleMultiplier;
                //transform.DOScale(_initialScale, _triangleSettings.ScaleChangeTime)
                //   .OnComplete(() => transform.localScale = _initialScale);

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