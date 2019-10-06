using UnityEngine;

namespace Framework.Tools.FSM
{
    public abstract class FSMCondition : ScriptableObject
    {
        protected FSMAction Action;
        protected FSMController Controller;

        public virtual void Initialize(FSMAction action, FSMController controller)
        {
            Action = action;
            Controller = controller;
        }

        public abstract bool Check();
    }
}