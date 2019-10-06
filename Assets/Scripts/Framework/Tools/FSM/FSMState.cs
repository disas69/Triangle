using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.FSM
{
    [CreateAssetMenu(fileName = "FSMState", menuName = "FSM/FSMState")]
    public class FSMState : ScriptableObject
    {
        private FSMController _controller;

        [SerializeField] private string _name;
        [SerializeField] private FSMAction _action;
        [SerializeField] private List<FSMTransition> _transitions = new List<FSMTransition>();

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public FSMAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public List<FSMTransition> Transitions
        {
            get { return _transitions; }
            set { _transitions = value; }
        }

        public void Initialize(FSMController controller)
        {
            _controller = controller;

            if (_action != null)
            {
                _action.Initialize(controller);
            }

            for (var i = 0; i < _transitions.Count; i++)
            {
                _transitions[i].Condition.Initialize(_action, controller);
            }
        }

        public void Enter()
        {
            if (_action != null)
            {
                _action.OnEnter();
            }
        }

        public void Update()
        {
            if (_action != null)
            {
                _action.OnUpdate();
            }

            CheckTransitions();
        }

        public void Exit()
        {
            if (_action != null)
            {
                _action.OnExit();
            }
        }

        private void CheckTransitions()
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                var conditionSucceeded = _transitions[i].Condition.Check();
                if (conditionSucceeded)
                {
                    _controller.TransitionToState(_transitions[i].State);
                    break;
                }
            }
        }
    }
}