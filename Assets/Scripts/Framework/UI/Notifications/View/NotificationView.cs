using System.Collections;
using Framework.UI.Notifications.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace Framework.UI.Notifications.View
{
    public abstract class NotificationView : MonoBehaviour
    {
        public abstract void Initialize(INotification model, float showTime);
        public abstract void Show();
        public abstract void Hide();
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class NotificationView<T> : NotificationView where T : class, INotification
    {
        private CanvasGroup _canvasGroup;
        private Coroutine _transitionCoroutine;
        private float _elapsedTime;
        private bool _isActive;

        [UsedImplicitly] public float TransitionSpeed = 1f;
        [UsedImplicitly] public bool InTransitionEnabled;
        [UsedImplicitly] public bool OutTransitionEnabled;

        protected T Model;
        protected float ShowTime;

        public override void Initialize(INotification model, float showTime)
        {
            Model = model as T;
            ShowTime = showTime;

            _canvasGroup = GetComponent<CanvasGroup>();
            _elapsedTime = 0f;
            _isActive = true;
        }

        public override void Show()
        {
            if (InTransitionEnabled)
            {
                StartTransition(InTransition());
            }
            else
            {
                _canvasGroup.alpha = 1f;
            }
        }

        public override void Hide()
        {
            if (OutTransitionEnabled)
            {
                StartTransition(OutTransition());
            }
            else
            {
                Destroy();
            }
        }

        public virtual void Update()
        {
            if (!_isActive || ShowTime < 0f)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= ShowTime)
            {
                _isActive = false;
                Hide();
            }
        }

        private void StartTransition(IEnumerator transition)
        {
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
            }

            _transitionCoroutine = StartCoroutine(transition);
        }

        private IEnumerator InTransition()
        {
            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += TransitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
        }

        private IEnumerator OutTransition()
        {
            _canvasGroup.alpha = 1f;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= TransitionSpeed * Time.deltaTime;
                yield return null;
            }

            _transitionCoroutine = null;
            Destroy();
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}