using EnemyAI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyAI
{
    public class PatrolSetup : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private Patrol patrol;
        [SerializeField] private int patrolPointCount;

        [Button("Save Patrol")]
        void SavePatrol() {
            patrol.patrolData = new PatrolData[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++) {
                patrol.patrolData[i] = new PatrolData() {
                    patrolPosition = patrolPoints[i].position,
                    patrolFace = patrolPoints[i].right
                };
            }
        }

        [Button("Load Patrol")]
        void LoadPatrol() {

            for (int i = 0; i < patrolPoints.Length; i++) {
                patrolPoints[i].position = patrol.patrolData[i].patrolPosition;
                patrolPoints[i].up = patrol.patrolData[i].patrolFace;
            }

            patrolPointCount = patrol.patrolData.Length;
        }



        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            for (int i = 0; i < patrolPointCount; i++) {
                Gizmos.DrawSphere(patrolPoints[i].position, 0.5f);
            }
        }
    }
}