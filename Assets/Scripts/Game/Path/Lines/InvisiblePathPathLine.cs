using System.Collections;
using DG.Tweening;
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

        protected override void OnLineTriggered()
        {
            //Do nothing
        }

        public override void SetProgress(Vector3 position)
        {
            //Do nothing
        }

        private void StopEffect()
        {
            this.SafeStopCoroutine(_effectCoroutine);
            _particleEffect.SetActive(false);
            MeshRenderer.material.DOPause();
        }

        private IEnumerator PlayEffect()
        {
            var time = Random.Range(Settings.MinInvisibleTime, Settings.MaxInvisibleTime);
            var waiter = new WaitForSeconds(time);
            yield return waiter;

            var color = MeshRenderer.sharedMaterial.color;

            while (true)
            {
                MeshRenderer.material.DOColor(color.WithAlpha(0), time);
                yield return waiter;

                MeshRenderer.material.DOColor(color.WithAlpha(1), time);
                yield return waiter;
            }
        }
    }
}