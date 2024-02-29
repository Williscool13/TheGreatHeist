using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Attention")]
    public class AttentionAction : EnemyStateAction
    {
        public override void Enter(EnemyStateMachine machine) {
        }

        public override void Execute(EnemyStateMachine machine) {
            machine.Attention.DecrementAttention(Time.deltaTime);
        }

        public override void Exit(EnemyStateMachine machine) {
        }
    }
}
