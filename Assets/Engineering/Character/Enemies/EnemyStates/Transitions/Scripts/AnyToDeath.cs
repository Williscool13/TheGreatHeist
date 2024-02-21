using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToDeath")]
    public class AnyToDeath : EnemyStateDecision
    {
        public override bool Decide(EnemyStateMachine machine) {
            if (machine.HealthSystem.IsDead) {
                return true;
            }
            return false;
        }
    }
}
