using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static EnemyStateMachine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToShoot")]
    public class AnyToShoot : EnemyStateDecision
    {
        [SerializeField] private float attentionDistanceCutoff = 0.5f;
        [SerializeField] private float maxAttentionStrength = 5.0f;
        [SerializeField] private float cutoffAttentionStrength = 2.5f;
        [SerializeField] private float minAttentionStrength = 0.2f;
        public override bool Decide(EnemyStateMachine machine) {
            // have this be over time and not instant. if off screen, have an arrow on player screen to show enemy spots player
            RaycastHit2D[] rhs = Physics2D.CircleCastAll(machine.GetWorldShootPoint(), machine.ViewDistance, Vector2.zero, 0, machine.TargetLayerMask);

            foreach (RaycastHit2D rh in rhs) {
                IHitbox tHitbox = rh.transform.GetComponentInChildren<IHitbox>();
                if (tHitbox == null) continue;

                foreach (Vector2 hitpoint in tHitbox.GetAllHitPoints()) {
                    Vector2 shootDir = (hitpoint - machine.GetWorldShootPoint()).normalized;
                    float dotProd = Vector2.Dot(shootDir, machine.Aim.GetAimDirection().normalized);
                    if (dotProd < Mathf.Cos(machine.FieldOfView / 2 * Mathf.Deg2Rad)) continue;
                    RaycastHit2D hit = Physics2D.Raycast(machine.GetWorldShootPoint(), shootDir, machine.ViewDistance, machine.ObstacleTargetLayerMask);
                    
                    if (hit.collider != null && hit.collider.CompareTag("Player")) {

                        float distance = Vector2.Distance(machine.GetWorldShootPoint(), hit.point);
                        float half = machine.ViewDistance * attentionDistanceCutoff;

                        float attenuation = Mathf.Lerp(cutoffAttentionStrength, minAttentionStrength,
                                Mathf.Clamp01((distance - half) / (machine.ViewDistance - half)));
                        
                        machine.Attention.IncreaseAttention(
                            attenuation * Time.deltaTime, 
                            rh.point, 
                            rh.transform, 
                            tHitbox);
                        
                        if (machine.Attention.IsAttentionAlerted()) {
                            return true; 
                        }
                    }
                }
            }
            return false;

        }
    }
}