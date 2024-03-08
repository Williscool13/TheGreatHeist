using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Investigate")]
    public class InvestigateAction : EnemyStateAction
    {
        [SerializeField] EnemyState idleAlerted;

        public override void Enter(EnemyStateMachine machine) {
            machine.Movement.SetMovementProperties(machine.InvestigateMoveSpeed, CharacterMovement.Movement.Standard);

            Debug.Log("adding investigate path");
            machine.AddInvestigatePath(machine.transform.position);

            machine.InvestigateTimestamp = Time.time + machine.InvestigateTimeout;

            //machine.AddInvestigatePath(machine.LastKnownPosition);
        }

        public override void Execute(EnemyStateMachine machine) {
            Vector2 dir = machine.LastKnownPosition - (Vector2)machine.transform.position;
            machine.Aim.RotateTowards(dir);
            float face = Vector2.Dot(machine.Aim.GetAimDirection().normalized, dir.normalized);
            if (face < 0.9) return; 
            if (Vector2.Distance(machine.transform.position, machine.LastKnownPosition) < 0.5f) {
                machine.Movement.SetMovementProperties(0, CharacterMovement.Movement.None);
                machine.Movement.StopMovement();
                machine.ChangeState(idleAlerted);
                // add current position to the queue of move commands
                return;
            }

            Vector2 moveDir = machine.LastKnownPosition - (Vector2)machine.transform.position;
            machine.Movement.Move(moveDir.normalized);
            Debug.Log("Moving towards " + machine.LastKnownPosition + ", the last known position");

            
            // if at the last known position, change state to idle alerted 
            // in idle alerted, the enemy will look around for a few seconds before walking back through the queue of move commands to the next patrol point
        }

        public override void Exit(EnemyStateMachine machine) {
            // add the current move command to the queue of move commands
            machine.Movement.SetMovementProperties(0, CharacterMovement.Movement.None);
            machine.Movement.StopMovement();
        }

    }

}