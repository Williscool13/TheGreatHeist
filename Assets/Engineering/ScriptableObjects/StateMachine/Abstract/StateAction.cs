using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachine {
    public abstract class StateAction<StateMachine> : ScriptableObject
{
        public abstract void Enter(StateMachine machine);
        public abstract void Execute(StateMachine machine);
        public abstract void Exit(StateMachine machine);
    }
}