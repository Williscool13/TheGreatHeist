using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{

    [CreateAssetMenu(menuName = "Player/Actions/Shoot")]
    public class ShootAction : PlayerStateAction
    {
        public override void Enter(PlayerStateMachine machine) { }

        public override void Execute(PlayerStateMachine machine) {
            if (!machine.CanShoot()) {
                return;
            }

            machine.Shoot();
        }

        public override void Exit(PlayerStateMachine machine) { }
    }

}
