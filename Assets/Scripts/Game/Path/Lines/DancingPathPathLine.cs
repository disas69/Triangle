using System.Collections;
using DG.Tweening;
using Framework.Extensions;
using Game.Path.Lines.Base;
using Game.Path.Lines.Configuration;
using UnityEngine;

namespace Game.Path.Lines
{
    public class DancingPathPathLine : PathLine<DancingPathLineSettings>
    {
        private Vector3? _initialScale;
        private Coroutine _effectCoroutine;

        public override LineType LineType
        {
            get { return LineType.Dancing; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _initialScale = null;
            _effectCoroutine = StartCoroutine(PlayEffect());
        }

        protected override void OnLinePassed()
        {
            base.OnLinePassed();
            StopEffect();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopEffect();
        }

        private void StopEffect()
        {
            this.SafeStopCoroutine(_effectCoroutine);

            transform.DOPause();

            if (_initialScale.HasValue)
            {
                transform.localScale = _initialScale.Value;
            }
        }

        private IEnumerator PlayEffect()
        {
            var time = Random.Range(Settings.MinScaledTime, Settings.MaxScaledTime);
            var waiter = new WaitForSeconds(time);
            yield return waiter;

            var scaleMultiplier = Random.Range(Settings.MinScaleMultiplier, Settings.MaxScaleMultiplier);
            _initialScale = transform.localScale;

            while (true)
            {
                var localScale = transform.localScale;
                var scale = new Vector3(localScale.x * scaleMultiplier, localScale.y * scaleMultiplier, localScale.z);
                transform.DOScale(scale, time);
                yield return waiter;

                transform.DOScale(_initialScale.Value, time);
                yield return waiter;
            }
        }
    }
}