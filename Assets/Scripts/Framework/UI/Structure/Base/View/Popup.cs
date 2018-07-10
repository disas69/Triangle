using System.Collections;
using UnityEngine;

namespace Framework.UI.Structure.Base.View
{
    public class Popup : Screen
    {
        private Coroutine _closeCoroutine;

        public override void Close()
        {
            if (_closeCoroutine == null)
            {
                _closeCoroutine = StartCoroutine(CloseInternal());
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Destroy(gameObject);
        }

        private IEnumerator CloseInternal()
        {
            OnExit();
            while (IsInTransition)
            {
                yield return null;
            }
        }
    }
}