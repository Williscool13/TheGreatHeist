using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace EnemyFiniteStateMachine
{
    [CreateAssetMenu(menuName = "Enemy/Actions/Shoot")]
    public class EnemyShootAction : EnemyStateAction
    {
        [SerializeField] EnemyState investigate;
        [SerializeField] EnemyState backtracking;

        [SerializeField] private bool instantlyKillOnSight = false;

        public override void Enter(EnemyStateMachine machine) {
            machine.DisableCorporeality();
            machine.CurrentHitboxHitpointIndex = -1;
            //machine.AddInvestigatePath(machine.transform.position);

            if (!instantlyKillOnSight) { return; }

            RaycastHit2D[] rhs = Physics2D.CircleCastAll(machine.GetWorldShootPoint(), machine.ViewDistance, Vector2.zero, 0, machine.TargetLayerMask);

            if (rhs.Length == 0) { return; }
            HealthSystem hs = rhs[0].transform.GetComponentInChildren<HealthSystem>();
            if (hs == null) { return; }

            hs.Damage(new DamageData(1000, hs.transform.position, hs.transform.position - machine.transform.position, 
                Vector2.Distance(hs.transform.position, machine.transform.position), false, Time.time, DamageImpactType.None));

            machine.ChangeState(backtracking);
        }

        public override void Execute(EnemyStateMachine machine) {

            Vector2[] ps = machine.TargetHitbox.GetAllHitPoints();
            // if player in LoS, turn to face player
            if (machine.CurrentHitboxHitpointIndex == -1) {
                HitboxHitpointData data = FindContact(ps, machine.GetWorldShootPoint(), machine);
                if (data.hitpointFound) {
                    machine.CurrentHitboxHitpointIndex = data.hitpointIndex;
               } else {
                    // check if player is still within vision range, and 
                    Debug.Log("No player hitbox points within LoS, moving to investigate last known position");
                    machine.ChangeState(investigate);
                    return;
                }
            } 
            
            Vector2 direction = ps[machine.CurrentHitboxHitpointIndex] - machine.GetWorldShootPoint();
            RaycastHit2D hit = Physics2D.Raycast(machine.GetWorldShootPoint(), direction, machine.ShootRange, machine.ObstacleTargetLayerMask);
            if (hit.collider != null && hit.collider.CompareTag("Player")) {
                machine.Aim.RotateTowards(direction);
                machine.SetInvestigationPoint(hit.point);
            } else {
                // hit point no longer in line of sight, will try to find another hit point in next frame
                machine.CurrentHitboxHitpointIndex = -1;
                return;
            }
            

            if (!machine.CanShoot()) { return; }

            machine.Shoot();

        }

        HitboxHitpointData FindContact(Vector2[] ps, Vector2 source, EnemyStateMachine machine) {
            for (int i = 0; i < ps.Length; i++) {
                RaycastHit2D hit = Physics2D.Raycast(source, ps[i] - source, machine.ShootRange, machine.ObstacleTargetLayerMask);
                if (hit.collider == null) continue;
                if (hit.collider.transform.CompareTag("Player")) {
                    return new HitboxHitpointData { hitpointFound = true, hitpointIndex = i };
                }
            }
            return new HitboxHitpointData { hitpointFound = false, hitpointIndex = -1 };
        }

        struct HitboxHitpointData {
            public bool hitpointFound;
            public int hitpointIndex;
        }

        public override void Exit(EnemyStateMachine machine) {
            machine.EnableCorporeality();
        }
    }
}