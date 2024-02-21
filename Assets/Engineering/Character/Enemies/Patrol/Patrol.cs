using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyAI
{
    [CreateAssetMenu(fileName = "Patrol", menuName = "Enemy/AI/Patrol")]
    public class Patrol : ScriptableObject
    {
        public PatrolData[] patrolData;
    }

}
[Serializable]
public struct PatrolData
{
    public Vector2 patrolPosition;
    public Vector2 patrolFace;
}