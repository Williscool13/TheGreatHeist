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
            // if user is pressing interact button, interact with object (use interact manager in state machine)

            Debug.Log("[Player State Machine] Attempting to Inteact");
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }

}
