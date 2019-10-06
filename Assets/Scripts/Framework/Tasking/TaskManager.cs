using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tasking
{
    public class TaskManager
    {
        private readonly Queue<Task> _tasks = new Queue<Task>();
        private readonly MonoBehaviour _host;

        private Task _currentTask;

        public TaskManager(MonoBehaviour host)
        {
            _host = host;
        }

        public void AddTask(IEnumerator action, Action callback)
        {
            _tasks.Enqueue(new Task(_host, action).Subscribe(callback));

            if (_currentTask == null)
            {
                UpdateQueue();
            }
        }

        public void Break()
        {
            if (_currentTask != null)
            {
                _currentTask.Stop();
                _currentTask = null;
            }
        }

        public void Restore()
        {
            UpdateQueue();
        }

        public void Clear()
        {
            Break();
            _tasks.Clear();
        }

        private void UpdateQueue()
        {
            _currentTask = GetNextTask();
            
            if (_currentTask != null)
            {
                _currentTask.Subscribe(UpdateQueue).Start();
            }
        }

        private Task GetNextTask()
        {
            if (_tasks.Count > 0)
            {
                return _tasks.Dequeue();
            }

            return null;
        }
    }
}