using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Backtrack")]
    public class BacktrackAction : EnemyStateAction
    {
        public override void Enter(EnemyStateMachine machine) {
        }

        public override void Execute(EnemyStateMachine machine) {
            Debug.Log("backtrack duck 1");
            if (machine.IsBacktracking()) return;
            Debug.Log("backtrack duck 2");


            machine.Backtrack();
        }

        public override void Exit(EnemyStateMachine machine) {
            machine.StopBacktrack();
        }
    }

}
