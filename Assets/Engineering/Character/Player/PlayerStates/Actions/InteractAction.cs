using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Interact")]
    public class InteractAction : PlayerStateAction
    {
        public override void Enter(PlayerStateMachine machine) {
        }

        public override void Execute(PlayerStateMachine machine) {
            machine.Highlight();
            if (machine.CanInteract()) {
                machine.Interact();
            }
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }

}
