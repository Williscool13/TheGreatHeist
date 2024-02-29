using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/AimMoveDirection")]
    public class AimMoveDirectionAction : PlayerStateAction
    {
        [SerializeField] private float crossHairMoveSpeed = 1.0f;
        [SerializeField] private float characterRotationSpeed = 1080f;
        public override void Enter(PlayerStateMachine machine) {
            machine.SetAimProperties(crossHairMoveSpeed, characterRotationSpeed);
        }

        public override void Execute(PlayerStateMachine machine) {
            if (machine.IsStunned()) { return; }

            Vector2 lookDir = machine.GetLastMovementDirection();
            machine.CustomLook(lookDir);
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }
}