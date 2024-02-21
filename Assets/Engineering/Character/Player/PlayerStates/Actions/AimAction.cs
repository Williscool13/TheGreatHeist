using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/Aim")]
    public class AimAction : PlayerStateAction
    {
        [SerializeField] private float crossHairMoveSpeed = 0.35f;
        [SerializeField] private float characterRotationSpeed = 1080f;
        public override void Enter(PlayerStateMachine machine) {
            machine.SetAimProperties(crossHairMoveSpeed, characterRotationSpeed);
        }

        public override void Execute(PlayerStateMachine machine) {
            machine.Look();
        }

        public override void Exit(PlayerStateMachine machine) {
        }
    }

}