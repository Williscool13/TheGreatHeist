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

            // disable attention component, aim component, and movement component
            machine.DisableAllEnemyComponents();
            
            // set color of outline to black
            machine.Attention.SetSpriteOutlineColor(Color.black);

            // set corporeality to 0 after some time and destroy the object after a few
            machine.Corporeality.TurnIncorporeal();

        }

        public override void Execute(EnemyStateMachine machine) {
        }

        public override void Exit(EnemyStateMachine machine) {
        }
    }
}
