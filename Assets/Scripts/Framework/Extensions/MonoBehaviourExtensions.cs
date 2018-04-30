using System;
using System.Collections;
using UnityEngine;

namespace Framework.Extensions
{
    public static class MonoBehaviourExtensions
    {
        public static Coroutine WaitForSeconds(this MonoBehaviour behaviour, float seconds, Action action)
        {
            return behaviour.StartCoroutine(WaitForSeconds(seconds, action));
        }

        private static IEnumerator WaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            if (action != null)
            {
                action.Invoke();
            }
        }

        public static void SafeStopCoroutine(this MonoBehaviour monoBehaviour, Coroutine coroutine)
        {
            if (coroutine != null)
            {
                monoBehaviour.StopCoroutine(coroutine);
            }
        }
    }
}