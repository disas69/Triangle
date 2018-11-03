using UnityEngine;

namespace Framework.Tools.FSM
{
    public abstract class FSMAction : ScriptableObject
    {
        protected FSMController Controller;

        public abstract bool IsFinished { get; }

        public virtual void Initialize(FSMController controller)
        {
            Controller = controller;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();
    }
}