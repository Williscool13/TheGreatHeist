using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToInvestigateLastDamage")]
    public class AnyToInvestigateLastDamage : EnemyStateDecision {

        [SerializeField] private float maxReactionDelay = 0.5f;
        [SerializeField] private float minReactionDelay = 0.1f;
        public override bool Decide(EnemyStateMachine machine) {
            if (machine.HealthSystem.LastDamageData == null) return false;
            if (machine.HealthSystem.LastDamageData.acknowledged) return false;


            float t = Time.time - machine.HealthSystem.LastDamageData.bulletShotTimestamp;
            if (t <= minReactionDelay || t >= maxReactionDelay) {
                return false;
            }

            machine.HealthSystem.AcknowledgeDamageData();

            Vector2 targetPos = machine.HealthSystem.LastDamageData.position - machine.HealthSystem.LastDamageData.direction * 10;
            //machine.SetInvestigationPoint(targetPos);
            machine.Attention.IncreaseAttention(0.5f, targetPos);
            Debug.Log("InvestigateBullet");
            return true;
        }

    }

}
