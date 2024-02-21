using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToInvestigateBullet")]
    public class AnyToInvestigateBullet : EnemyStateDecision {

        [SerializeField] private float maxReactionDelay = 0.5f;
        [SerializeField] private float minReactionDelay = 0.1f;
        public override bool Decide(EnemyStateMachine machine) {
            if (machine.HealthSystem.LastDamageData == null) return false;

            float t = Time.time - machine.HealthSystem.LastDamageData.bulletShotTimestamp;
            if (t > minReactionDelay && t < maxReactionDelay) {
                Vector2 targetPos = machine.HealthSystem.LastDamageData.position - machine.HealthSystem.LastDamageData.direction * 10;
                machine.SetInvestigationPoint(targetPos);
                return true;
            }

            return false;
        }

    }

}
