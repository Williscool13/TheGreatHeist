using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Decisions/AnyToInvestigateFlare")]
    public class AnyToInvestigateFlare : EnemyStateDecision
    {
        public override bool Decide(EnemyStateMachine machine) {
            Collider2D[] flares = Physics2D.OverlapCircleAll(machine.transform.position, machine.ViewDistance, machine.FlareLayerMask);
            if (flares.Length > 0) {
                for (int i = 0; i < flares.Length; i++) {
                    if (machine.PreviousInvestigatedFlares.Contains(flares[i].gameObject)) {
                        continue;
                    }

                    Vector2 dir = (flares[0].transform.position - machine.transform.position).normalized;

                    RaycastHit2D hit = Physics2D.Raycast(machine.GetWorldShootPoint(), dir, machine.ViewDistance, machine.ObstacleFlareLayerMask);
                    if (hit.collider != null && hit.collider.CompareTag("Flare")) {
                        machine.PreviousInvestigatedFlares.Add(flares[i].gameObject);
                        flares[i].GetComponent<LightFlare>().OnFlareRelease += machine.OnFlareReleased_RemoveFromInvestigatedFlares; 
                        //machine.SetInvestigationPoint((Vector2)flares[i].transform.position - dir);
                        machine.Attention.IncreaseAttention(0.5f, flares[i].transform.position);
                        
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
