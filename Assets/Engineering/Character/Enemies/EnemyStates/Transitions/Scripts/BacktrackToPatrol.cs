using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/BacktrackToPatrol")]
    public class BacktrackToPatrol : EnemyStateDecision
    {
        public override bool Decide(EnemyStateMachine machine) {
            return machine.backtrackCount == 0;
        }
    }

}
