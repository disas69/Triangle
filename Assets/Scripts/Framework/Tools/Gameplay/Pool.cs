using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.Gameplay
{
    public class Pool<T> where T : Component
    {
        private readonly T _itemPrefab;
        private readonly Transform _poolRoot;
        private readonly Transform _parentObject;
        private readonly Queue<T> _itemsQueue;

        public int Count
        {
            get { return _itemsQueue.Count; }
        }

        public Pool(T itemPrefab, Transform parentObject, int poolCapacity)
        {
            _itemPrefab = itemPrefab;
            _parentObject = parentObject;
            _itemsQueue = new Queue<T>(poolCapacity);

            _poolRoot = new GameObject(string.Format("[{0}] Pool", itemPrefab.GetType().Name)).transform;
            _poolRoot.SetParent(parentObject);

            for (int i = 0; i < poolCapacity; i++)
            {
                var item = Object.Instantiate(itemPrefab, _poolRoot);
                item.gameObject.SetActive(false);

                _itemsQueue.Enqueue(item);
            }
        }

        public T GetNext()
        {
            T item;

            if (_itemsQueue.Count > 0)
            {
                item = _itemsQueue.Dequeue();
                item.transform.SetParent(_parentObject);
                item.gameObject.SetActive(true);
            }
            else
            {
                item = Object.Instantiate(_itemPrefab, _parentObject);
            }

            return item;
        }

        public void Return(T item)
        {
            item.gameObject.SetActive(false);
            item.transform.position = Vector3.zero;
            item.transform.localEulerAngles = Vector3.zero;
            item.transform.SetParent(_poolRoot);

            _itemsQueue.Enqueue(item);
        }
    }
}