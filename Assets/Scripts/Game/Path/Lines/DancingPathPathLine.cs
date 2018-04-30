using System.Collections;
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

            if (_initialScale.HasValue)
            {
                transform.localScale = _initialScale.Value;
            }
        }

        private IEnumerator PlayEffect()
        {
            var waiter = new WaitForSeconds(Random.Range(Settings.MinScaledTime, Settings.MaxScaledTime));
            yield return waiter;

            var scaleMultiplier = Random.Range(Settings.MinScaleMultiplier,
                Settings.MaxScaleMultiplier);
            _initialScale = transform.localScale;

            while (true)
            {
                var localScale = transform.localScale;
                transform.localScale = new Vector3(localScale.x * scaleMultiplier, localScale.y * scaleMultiplier,
                    localScale.z);
                yield return waiter;

                transform.localScale = _initialScale.Value;
                yield return waiter;
            }
        }
    }
}