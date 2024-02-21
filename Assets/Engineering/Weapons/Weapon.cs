using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform muzzlePosition;
    [SerializeField] protected ParticleSystem muzzleFlash;

    public abstract bool CanShoot(bool press);
    public abstract void Shoot(Vector2 target);
    public abstract void Equip();
    public abstract void Unequip();

}
