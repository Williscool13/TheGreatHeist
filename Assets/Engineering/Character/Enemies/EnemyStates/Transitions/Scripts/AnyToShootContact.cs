using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToShootContact")]
    public class AnyToShootContact : EnemyStateDecision
    {
        [SerializeField] private float maxReactionDelay = 0.5f;
        [SerializeField] private float minReactionDelay = 0.1f;
        public override bool Decide(EnemyStateMachine machine) {
            if (!machine.PreviousCollisionData.isColliding) return false;
            float t = Time.time - machine.PreviousCollisionData.collisionTimestamp;
            if (t > minReactionDelay && t < maxReactionDelay) {
                Vector2 targetPos = machine.PreviousCollisionData.collisionPoint;
                machine.Attention.IncreaseAttention(
                    10.0f, 
                    targetPos, 
                    machine.PreviousCollisionData.collision.transform, 
                    machine.PreviousCollisionData.collision.transform.GetComponentInChildren<IHitbox>());

                if (machine.Attention.IsAttentionAlerted()) {
                    return true;
                }
            }

            return false;

        }
    }
}
