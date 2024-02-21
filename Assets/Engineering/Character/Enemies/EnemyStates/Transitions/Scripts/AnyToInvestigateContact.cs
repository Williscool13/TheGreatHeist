using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToInvestigateContact")]
    public class AnyToInvestigateContact : EnemyStateDecision {

        [SerializeField] private float maxReactionDelay = 0.5f;
        [SerializeField] private float minReactionDelay = 0.1f;
        public override bool Decide(EnemyStateMachine machine) {
            Debug.Log("Contact Duck 1");
            if (!machine.PreviousCollisionData.isColliding) return false;
            Debug.Log("Contact Duck 2");
            float t = Time.time - machine.PreviousCollisionData.collisionTimestamp;
            if (t > minReactionDelay && t < maxReactionDelay) {
                Debug.Log("Contact Duck 3");
                Vector2 targetPos = machine.PreviousCollisionData.collisionPoint;
                machine.SetInvestigationPoint(targetPos);
                return true;
            }

            return false;

        }

    }

}
