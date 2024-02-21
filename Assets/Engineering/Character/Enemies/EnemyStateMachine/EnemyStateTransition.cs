using FiniteStateMachine;
using PlayerFiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/State Transition")]
    public class EnemyStateTransition : BaseStateTransition<EnemyStateMachine> {
        public EnemyStateDecision decision;
        public EnemyState trueState;
        public EnemyState falseState;

        public override void Execute(EnemyStateMachine machine) {

            if (decision.Decide(machine)) {
                if (trueState is not EnemyState_Remain) {
                    machine.ChangeState(trueState);
                }
            }
            else {
                if (falseState is not EnemyState_Remain) {
                    machine.ChangeState(falseState);
                }
            }
        }
    }
}
