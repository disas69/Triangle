using System;
using System.Collections;
using Framework.Extensions;
using Framework.UI.Structure.Base.View;
using UnityEngine;

namespace Game.UI.Screens.Start
{
    public class StartPage : Page<StartPageModel>
    {
        private Coroutine _overlayTransitionCoroutine;

        [SerializeField] private float _overlayTransitionSpeed;
        [SerializeField] private CanvasGroup _overlay;

        protected override IEnumerator InTransition(Action callback)
        {
            _overlayTransitionCoroutine = StartCoroutine(ShowOverlay());
            yield return _overlayTransitionCoroutine;
        }

        private IEnumerator ShowOverlay()
        {
            _overlay.gameObject.SetActive(true);
            _overlay.alpha = 1f;

            while (_overlay.alpha > 0f)
            {
                _overlay.alpha -= _overlayTransitionSpeed * 2f * Time.deltaTime;
                yield return null;
            }

            _overlay.alpha = 0f;
            _overlay.gameObject.SetActive(false);
            _overlayTransitionCoroutine = null;
        }

        public override void OnExit()
        {
            _overlay.gameObject.SetActive(false);
            this.SafeStopCoroutine(_overlayTransitionCoroutine);
            base.OnExit();
        }
    }
}