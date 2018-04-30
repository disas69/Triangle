using System;
using Framework.Extensions;
using Game.Path.Configuration;
using UnityEngine;

namespace Game.Path.Lines.Base
{
    public abstract class Line : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private BoxCollider _boxCollider;
        private bool _isVisible;
        private bool _isTriggered;
        private bool _isPassed;
        private float _length;

        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _passedMaterial;
        [SerializeField] private Material _tempMaterial;

        public event Action<Line> OnBecameVisible;
        public event Action<Line> OnBecameInvisible;

        public abstract LineType LineType { get; }
        public bool Countable { get; private set; }

        protected MeshRenderer MeshRenderer
        {
            get { return _meshRenderer; }
        }

        public Vector3 EndPointPosition
        {
            get { return _endPoint.position; }
        }

        public bool IsTriggered
        {
            get { return _isTriggered; }
            set
            {
                _meshRenderer.material = value ? _tempMaterial : _defaultMaterial;
                _isTriggered = value;

                if (_isTriggered)
                {
                    OnLineTriggered();
                }
            }
        }

        public bool IsPassed
        {
            get { return _isPassed; }
            set
            {
                _meshRenderer.material = value ? _passedMaterial : _defaultMaterial;
                _isPassed = value;

                if (_isPassed)
                {
                    OnLinePassed();
                }
            }
        }

        public void Setup(Settings settings)
        {
            _length = settings.Length;

            var scale = transform.localScale;
            scale.z = _length;

            SetupPoints();

            transform.localScale = scale;
            transform.localEulerAngles = new Vector3(0f, settings.RotationAngle, 0f);

            Countable = settings.Countable;
            IsTriggered = false;
            IsPassed = false;
        }

        public void SetPosition(Vector3 startPointPosition)
        {
            transform.position = startPointPosition;
            transform.position = _endPoint.position;

            if (transform.position.z < 0)
            {
                _meshRenderer.material = _passedMaterial;
            }
        }

        public void SetProgress(Vector3 trianglePosition)
        {
            var passedPathLength = (_startPoint.position - trianglePosition).magnitude;
            _meshRenderer.material.Lerp(_defaultMaterial, _passedMaterial, passedPathLength / _length);
        }

        protected virtual void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _boxCollider = GetComponent<BoxCollider>();
            _isVisible = _meshRenderer.isVisible;

            if (_isVisible)
            {
                OnBecameVisible.SafeInvoke(this);
            }

            SetupPoints();
        }

        protected virtual void Update()
        {
            if (_meshRenderer.isVisible != _isVisible)
            {
                _isVisible = _meshRenderer.isVisible;

                if (_isVisible)
                {
                    OnLineBecameVisible();
                    OnBecameVisible.SafeInvoke(this);
                }
                else
                {
                    OnLineBecameInvisible();
                    OnBecameInvisible.SafeInvoke(this);
                }
            }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnLineTriggered()
        {
        }

        protected virtual void OnLinePassed()
        {
        }

        protected virtual void OnLineBecameVisible()
        {
        }

        protected virtual void OnLineBecameInvisible()
        {
        }

        private void SetupPoints()
        {
            var extents = _boxCollider.bounds.extents;
            extents.x = 0f;
            extents.y = 0f;

            _startPoint.position = _boxCollider.bounds.center - extents;
            _endPoint.position = _boxCollider.bounds.center + extents;
        }
    }
}