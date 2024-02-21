using FiniteStateMachine;
using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/State")]
    public class EnemyState : BaseState<EnemyStateMachine>
    {
        public List<EnemyStateAction> Actions = new List<EnemyStateAction>();
        public List<EnemyStateTransition> Transitions = new List<EnemyStateTransition>();
        public override void Enter(EnemyStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Enter(machine);
            }
        }
        public override void Exit(EnemyStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Exit(machine);
            }
        }

        public override void Execute(EnemyStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Execute(machine);
                if (machine.CurrentState != this) return;
            }

            for (int i = 0; i < Transitions.Count; i++) {
                Transitions[i].Execute(machine);
                if (machine.CurrentState != this) return;
            }

        }

    }

}
