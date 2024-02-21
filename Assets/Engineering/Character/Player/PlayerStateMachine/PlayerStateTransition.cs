using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/State Transition")]
    public class PlayerStateTransition : BaseStateTransition<PlayerStateMachine> {
        public PlayerStateDecision decision;
        public PlayerState trueState;
        public PlayerState falseState;

        public override void Execute(PlayerStateMachine machine) {
            if (decision.Decide(machine)) {
                if (trueState is not PlayerState_Remain) {
                    machine.ChangeState(trueState);
                }
            } else {
                if (falseState is not PlayerState_Remain) {
                    machine.ChangeState(falseState);
                }
            }
        }
    }
}

