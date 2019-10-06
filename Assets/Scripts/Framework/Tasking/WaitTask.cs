using System;
using System.Collections;
using UnityEngine;

namespace Framework.Tasking
{
    public class WaitTask : Task
    {
        public WaitTask(MonoBehaviour host, float seconds, Action action) : base(host, WaitForSeconds(seconds, action))
        {
        }

        private static IEnumerator WaitForSeconds(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            if (action != null)
            {
                action.Invoke();
            }
        }
    }
}