using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Invulnerable")]
    public class InvulnerableAction : PlayerStateAction
    {
        public override void Enter(PlayerStateMachine machine) {
            machine.SetInvulnerable(true);
        }

        public override void Execute(PlayerStateMachine machine) {
            Debug.Log("Player is invulnerable");
        }

        public override void Exit(PlayerStateMachine machine) {
            machine.SetInvulnerable(false);
        }

    }

}