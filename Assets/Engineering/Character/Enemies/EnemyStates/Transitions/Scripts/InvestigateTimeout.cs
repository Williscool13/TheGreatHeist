using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/InvestigateTimeout")]
    public class InvestigateTimeout : EnemyStateDecision
    {
        public override bool Decide(EnemyStateMachine machine) {
            if (machine.InvestigateTimestamp < Time.time) {
                machine.InvestigateTimestamp = 0;
                return true;
            }
            return false;
        }
    }
}
