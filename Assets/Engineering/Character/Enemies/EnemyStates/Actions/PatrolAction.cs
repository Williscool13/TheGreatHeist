using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Patrol")]
    public class PatrolAction : EnemyStateAction
    {
        public override void Enter(EnemyStateMachine machine) {
        }

        public override void Execute(EnemyStateMachine machine) {
            if (!machine.HasPatrolRoute()) {
                if (machine.Aim.Sweeping) return;

                if (machine.PatrolIdleTimer > 0) {
                    machine.PatrolIdleTimer -= Time.deltaTime;
                    return;
                }

                machine.Aim.RandomSweep(true);
                machine.PatrolIdleTimer = machine.PatrolIdleTime;

                return;
            }
                
            if (machine.IsPatrolling()) return;

            machine.IncremenetPatrolIndex();
            machine.Patrol(
                machine.PatrolRoute.patrolData[machine.PatrolIndex].patrolPosition,
                machine.PatrolRoute.patrolData[machine.PatrolIndex].patrolFace
                );
        }

        public override void Exit(EnemyStateMachine machine) {
            machine.StopPatrol();
            machine.Movement.StopMovement();
            machine.Aim.StopAllAim();
        }
    }
}