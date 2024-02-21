using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "HealthData", menuName = "Data/HealthData")]
public class HealthData : ScriptableObject
{
    public int maxHealth;
    public bool critImmune;
}
