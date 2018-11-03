using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.FSM
{
    public class FSMController : MonoBehaviour
    {
        private FSMState _currentState;

        [SerializeField] private List<FSMState> _states;

        public List<FSMState> States { get; private set; }

        public FSMState CurrentState
        {
            get { return _currentState; }
        }

        private void Start()
        {
            CreateStates();

            if (States.Count > 0)
            {
                _currentState = States[0];

                for (int i = 0; i < States.Count; i++)
                {
                    States[i].Initialize(this);
                }
            }
            else
            {
                Debug.Log(string.Format("Failed to initialize FSMController for \"{0}\"", gameObject.name));
            }
        }

        public void TransitionToState(FSMState nextState)
        {
            if (_currentState != null) _currentState.Exit();
            nextState.Enter();

            _currentState = nextState;
        }

        public void TransitionToState(string stateName)
        {
            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                if (state.Name == stateName)
                {
                    TransitionToState(state);
                }
            }
        }

        private void Update()
        {
            if (_currentState != null) _currentState.Update();
        }

        private void CreateStates()
        {
            States = new List<FSMState>();

            for (int i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                var fsmStateInstance = ScriptableObject.CreateInstance<FSMState>();
                fsmStateInstance.Name = state.Name;

                var action = state.Action;
                fsmStateInstance.Action = (FSMAction) ScriptableObject.CreateInstance(action.GetType());
                fsmStateInstance.Transitions = new List<FSMTransition>();

                for (int j = 0; j < state.Transitions.Count; j++)
                {
                    var transition = state.Transitions[j];
                    var condition = (FSMCondition) ScriptableObject.CreateInstance(transition.Condition.GetType());
                    fsmStateInstance.Transitions.Add(new FSMTransition {Condition = condition, StateName = transition.State.Name});
                }

                States.Add(fsmStateInstance);
            }

            for (int i = 0; i < States.Count; i++)
            {
                var stateInstance = States[i];

                for (int j = 0; j < stateInstance.Transitions.Count; j++)
                {
                    var transition = stateInstance.Transitions[j];
                    transition.State = States.Find(s => s.Name == transition.StateName);
                }
            }
        }
    }
}