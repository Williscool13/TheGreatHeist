using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Player/Actions/ShootFlare")]
    public class ShootFlareAction : PlayerStateAction
    {
        public override void Enter(PlayerStateMachine machine) { }

        public override void Execute(PlayerStateMachine machine) {
            if (!machine.CanFlare()) { return; }

            machine.Flare();
        }

        public override void Exit(PlayerStateMachine machine) { }
    }

}
