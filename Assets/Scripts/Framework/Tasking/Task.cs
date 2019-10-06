using System;
using System.Collections;
using Framework.Utils;
using UnityEngine;

namespace Framework.Tasking
{
    public class Task
    {
        private readonly MonoBehaviour _host;
        private readonly IEnumerator _action;

        private Action _callback;
        private Coroutine _coroutine;

        public static Task Create(IEnumerator taskAction)
        {
            return new Task(Scheduler.Instance, taskAction);
        }

        public Task(MonoBehaviour host, IEnumerator action)
        {
            _host = host;
            _action = action;
        }

        public void Start()
        {
            if (_coroutine == null)
            {
                _coroutine = _host.StartCoroutine(RunTask());
            }
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                _host.StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        public Task Subscribe(Action callback)
        {
            _callback += callback;
            return this;
        }

        private IEnumerator RunTask()
        {
            yield return _action;

            if (_callback != null)
            {
                _callback();
            }
        }
    }
}