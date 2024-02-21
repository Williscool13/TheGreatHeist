using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Footsteps")]
    public class FootstepAction : PlayerStateAction
    {
        [SerializeField] private float footstepCooldown;
        [SerializeField] private float stairFootstepCooldown;
        public override void Enter(PlayerStateMachine machine) {
        }

        public override void Execute(PlayerStateMachine machine) {
            if (machine.FootstepTimer > 0) {
                machine.FootstepTimer -= Time.deltaTime;
                return;
            }
            if (machine.GetLastMovementDirection().magnitude < 0.1f) {
                return;
            }

            machine.FootstepManager.PlayWalk();
            if (machine.FootstepManager.IsStairs) {
                machine.FootstepTimer = stairFootstepCooldown;
            }
            else {
                machine.FootstepTimer = footstepCooldown;
            }
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }
}
