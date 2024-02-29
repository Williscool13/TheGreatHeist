using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunStats", menuName = "Data/GunStats")]
public class GunStats : ScriptableObject
{
    // gun stats
    public int damage;
    public int maxAmmo;
    public float fireRate;
    public bool automatic;

    public int bounceCount;

    public float criticalChance;
    public bool alwaysCritFromBehind;

    // spread
    public float bulletSpread;
    public float bulletSpreadAngle;
    public float bulletSpreadAngleIncrease;
    public float bulletSpreadAngleMax;
    // decay
    public float bulletSpreadAngleDecay;
    public float bulletSpreadAngleDecayDelay;
    public float bulletSpreadAngleDecayDelayMax;
    public float bulletSpreadAngleDecayDelayMin;
}
