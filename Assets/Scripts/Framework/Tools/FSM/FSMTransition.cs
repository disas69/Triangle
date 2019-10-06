using System;
using UnityEngine;

namespace Framework.Tools.FSM
{
    [Serializable]
    public class FSMTransition
    {
        [SerializeField] private FSMCondition _condition;
        [SerializeField] private FSMState _state;

        public string StateName { get; set; }

        public FSMCondition Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        public FSMState State
        {
            get { return _state; }
            set { _state = value; }
        }
    }
}