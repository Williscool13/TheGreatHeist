using ScriptableObjectDependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/SweepToBacktrack")]
    public class SweepToBacktrack : EnemyStateDecision
    {
        [SerializeField] private IntegerReference sweepCount;
        public override bool Decide(EnemyStateMachine machine) {
            return machine.Aim.CurrentSweepCount >= sweepCount.Value;
        }
    }

}
