using FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/State")]
    public class PlayerState : BaseState<PlayerStateMachine>
    {
        public List<PlayerStateAction> Actions = new List<PlayerStateAction>();
        public List<PlayerStateTransition> Transitions = new List<PlayerStateTransition>();
        public override void Enter(PlayerStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Enter(machine);
            }
        }
        public override void Exit(PlayerStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Exit(machine);
            }
        }

        public override void Execute(PlayerStateMachine machine) {
            for (int i = 0; i < Actions.Count; i++) {
                Actions[i].Execute(machine);
            }

            for (int i = 0; i < Transitions.Count; i++) {
                Transitions[i].Execute(machine);
                if (machine.CurrentState != this) break;
            }
            
        }

    }
}