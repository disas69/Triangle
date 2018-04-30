using System.Collections;
using Framework.Extensions;
using Game.Path.Lines.Base;
using Game.Path.Lines.Configuration;
using UnityEngine;

namespace Game.Path.Lines
{
    public class InvisiblePathPathLine : PathLine<InvisiblePathLineSettings>
    {
        private Coroutine _effectCoroutine;

        [SerializeField] private GameObject _particleEffect;

        public override LineType LineType
        {
            get { return LineType.Invisible; }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            MeshRenderer.enabled = true;
            _particleEffect.SetActive(true);
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
            MeshRenderer.enabled = true;
            _particleEffect.SetActive(false);
        }

        private IEnumerator PlayEffect()
        {
            var waiter = new WaitForSeconds(Random.Range(Settings.MinInvisibleTime, Settings.MaxInvisibleTime));
            yield return waiter;

            while (true)
            {
                MeshRenderer.enabled = false;
                yield return waiter;

                MeshRenderer.enabled = true;
                yield return waiter;
            }
        }
    }
}