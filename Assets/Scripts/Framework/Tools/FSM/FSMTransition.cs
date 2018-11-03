using System;
using UnityEngine;

namespace Framework.Tools.FSM
{
    [Serializable]
    public class FSMTransition
    {
        private string _stateName;

        [SerializeField] private FSMCondition _condition;
        [SerializeField] private FSMState _state;

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

        public string StateName
        {
            get { return _stateName; }
            set { _stateName = value; }
        }
    }
}