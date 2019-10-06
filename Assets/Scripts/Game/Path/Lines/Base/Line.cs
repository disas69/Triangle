using System;
using Assets.Scripts.Game;
using Framework.Extensions;
using Game.Path.Configuration;
using UnityEngine;

namespace Game.Path.Lines.Base
{
    public abstract class Line : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private BoxCollider _boxCollider;
        private Animation _animation;
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

        public Vector3 StartPointPosition
        {
            get { return _startPoint.position; }
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
            if (!Countable)
            {
                _meshRenderer.sharedMaterial = _passedMaterial;
            }

            IsTriggered = false;
            IsPassed = false;
        }

        public void SetPosition(Vector3 startPointPosition)
        {
            transform.position = startPointPosition;
            transform.position = _endPoint.position;

            if (transform.position.z < 0)
            {
                _meshRenderer.sharedMaterial = _passedMaterial;
            }
        }

        public virtual void SetProgress(Vector3 position)
        {
            if (!Countable)
            {
                return;
            }

            var passedPathLength = GetPassedLength(position, this);
            var ratio = passedPathLength / _length;

            _meshRenderer.sharedMaterial.Lerp(_defaultMaterial, _passedMaterial, ratio);
        }

        public void ChangeMaterial()
        {
            _meshRenderer.sharedMaterial = _passedMaterial;
        }

        private float GetPassedLength(Vector3 position, Line line)
        {
            var a = Vector2.Distance(new Vector2(position.x, position.z),
                new Vector2(line.StartPointPosition.x, line.StartPointPosition.z));
            var b = MathUtils.GetDistanceToSegment(new Vector2(position.x, position.z),
                new Vector2(line.StartPointPosition.x, line.StartPointPosition.z),
                new Vector2(line.EndPointPosition.x, line.EndPointPosition.z));

            if (b > 0)
            {
                return Mathf.Sqrt(a * a + b * b);
            }

            return 0;
        }

        protected virtual void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _boxCollider = GetComponent<BoxCollider>();
            _animation = GetComponentInChildren<Animation>(true);
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
            _meshRenderer.sharedMaterial = _defaultMaterial;

            if (_animation != null)
            {
                _animation.gameObject.SetActive(false);
            }
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnLineTriggered()
        {
            if (Countable)
            {
                _meshRenderer.sharedMaterial = _tempMaterial;
            }
        }

        protected virtual void OnLinePassed()
        {
            _meshRenderer.sharedMaterial = _passedMaterial;

            if (Countable)
            {
                if (_animation != null)
                {
                    _animation.gameObject.SetActive(true);
                    _animation.Play();
                }
            }
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