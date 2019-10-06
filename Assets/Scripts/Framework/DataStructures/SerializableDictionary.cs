using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.DataStructures
{
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ISerializationCallbackReceiver
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

        [SerializeField] private List<TKey> _keys = new List<TKey>();
        [SerializeField] private List<TValue> _values = new List<TValue>();

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (_dictionary.TryGetValue(key, out value))
                {
                    return _dictionary[key];
                }

                Debug.LogError(string.Format("SerializableDictionary doesn't contain a value with the key: {0}", key));
                return default(TValue);
            }
            set { _dictionary[key] = value; }
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            for (var i = 0; i < _keys.Count; i++)
            {
                var key = _keys[i];
                var value = _values.Count > i ? _values[i] : default(TValue);

                if (!_dictionary.ContainsKey(key))
                {
                    _dictionary.Add(key, value);
                }
            }
        }
    }
}