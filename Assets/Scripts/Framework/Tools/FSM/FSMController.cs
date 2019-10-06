using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Framework.Tools.FSM
{
    public class FSMController : MonoBehaviour
    {
        [SerializeField] private List<FSMState> _states;

        public List<FSMState> States { get; private set; }
        public FSMState CurrentState { get; private set; }

        private void Start()
        {
            CreateStates();

            if (States.Count > 0)
            {
                CurrentState = States[0];

                for (var i = 0; i < States.Count; i++)
                {
                    States[i].Initialize(this);
                }
            }
            else
            {
                Debug.Log(string.Format("Failed to initialize FSMController for \"{0}\" as it has no states", gameObject.name));
            }
        }

        public void TransitionToState(FSMState nextState)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            nextState.Enter();
            CurrentState = nextState;
        }

        public void TransitionToState(string stateName)
        {
            for (var i = 0; i < States.Count; i++)
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
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }

        private void CreateStates()
        {
            States = new List<FSMState>();

            for (var i = 0; i < _states.Count; i++)
            {
                var state = _states[i];
                var stateInstance = ScriptableObject.CreateInstance<FSMState>();
                CopyPublicFields(state, stateInstance);
                stateInstance.Name = state.Name;

                var actionInstance = (FSMAction) ScriptableObject.CreateInstance(state.Action.GetType());
                CopyPublicFields(state.Action, actionInstance);
                stateInstance.Action = actionInstance;

                for (var j = 0; j < state.Transitions.Count; j++)
                {
                    var transition = state.Transitions[j];
                    var conditionInstance = (FSMCondition) ScriptableObject.CreateInstance(transition.Condition.GetType());
                    CopyPublicFields(transition.Condition, conditionInstance);
                    stateInstance.Transitions.Add(new FSMTransition {Condition = conditionInstance, StateName = transition.State.Name});
                }

                States.Add(stateInstance);
            }

            for (int i = 0; i < States.Count; i++)
            {
                var state = States[i];
                for (var j = 0; j < state.Transitions.Count; j++)
                {
                    var transition = state.Transitions[j];
                    transition.State = States.Find(s => s.Name == transition.StateName);
                }
            }
        }

        private static void CopyPublicFields<T>(T origin, T destination) where T : ScriptableObject
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            var type = origin.GetType();
            var fields = type.GetFields(flags);

            for (var i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                field.SetValue(destination, field.GetValue(origin));
            }
        }
    }
}