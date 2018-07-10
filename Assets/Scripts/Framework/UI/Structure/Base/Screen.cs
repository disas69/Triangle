using System;
using System.Collections;
using Framework.Extensions;
using UnityEngine;

namespace Framework.UI.Structure.Base
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Screen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _transitionCoroutine;
        private INavigationProvider _navigationProvider;

        [SerializeField] private bool _inTransition = true;
        [SerializeField] private bool _outTransition = true;
        [SerializeField] private float _transitionSpeed;

        public bool IsInTransition
        {
            get { return _transitionCoroutine != null; }
        }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual IEnumerator InTransition()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += _transitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
        }

        protected virtual IEnumerator OutTransition(Action callback)
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 1f;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= _transitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
            callback.SafeInvoke();
        }

        public void Initialize(INavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;
        }

        public virtual void OnEnter()
        {
            gameObject.SetActive(true);

            this.SafeStopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;

            if (_inTransition)
            {
                _transitionCoroutine = StartCoroutine(InTransition());
            }
        }

        public virtual void Update()
        {
        }

        public virtual void Close()
        {
            _navigationProvider.Back();
        }

        public virtual void OnExit()
        {
            this.SafeStopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;

            if (_outTransition)
            {
                _transitionCoroutine = StartCoroutine(OutTransition(() => { gameObject.SetActive(false); }));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void OnDestroy()
        {
        }
    }
}