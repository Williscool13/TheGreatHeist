using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Death")]
    public class EnemyDeathAction : EnemyStateAction
    {
        public override void Enter(EnemyStateMachine machine) {
            machine.Aim.StopAllAim();
            machine.Movement.StopMovement();
            machine.Movement.SetKinematic(true);
        }

        public override void Execute(EnemyStateMachine machine) {
        }

        public override void Exit(EnemyStateMachine machine) {
        }
    }
}
