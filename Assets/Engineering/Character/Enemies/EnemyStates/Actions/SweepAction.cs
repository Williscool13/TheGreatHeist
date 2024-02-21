using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/LookAround")]
    public class SweepAction : EnemyStateAction
    {
        [SerializeField] private IntegerReference sweepCount;
        public override void Enter(EnemyStateMachine machine) {
            machine.Aim.CurrentSweepCount = 0;
        
        }

        public override void Execute(EnemyStateMachine machine) {
            if (machine.Aim.Sweeping || machine.Aim.CurrentSweepCount >= sweepCount.Value) return;
            
            machine.Aim.Sweep(false);
             
        }

        public override void Exit(EnemyStateMachine machine) {
            machine.Aim.StopAllAim();
            machine.Movement.StopMovement();
        }
    }
}
