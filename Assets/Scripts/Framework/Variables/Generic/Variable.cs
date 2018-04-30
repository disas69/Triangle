using System;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Variables.Generic
{
    [Serializable]
    public class Variable<T> : ScriptableObject
    {
        [SerializeField] private T _value;

        public event Action<T> ValueChanged;

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                ValueChanged.SafeInvoke(_value);
            }
        }
    }
}