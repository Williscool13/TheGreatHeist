using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLoadoutManager : MonoBehaviour
{
    public LoadoutType CurrentLoadout { get; private set; } = LoadoutType.Unarmed;
    [SerializeField][ReadOnly] public Weapon CurrentWeapon { get; private set; } = null;


    [SerializeField] LoadoutConfig[] loadoutConfigs;


    [SerializeField][SceneObjectsOnly] Weapon startingWeapon;


    public event EventHandler<LoadoutChangeEventArgs> OnLoadoutChanged;

    void Start()
    {
        if (startingWeapon != null) { ChangeLoadout(startingWeapon.LoadoutType, startingWeapon); }
    }

    public void ChangeLoadout(LoadoutType loadout, Weapon weapon) {

        LoadoutChangeEventArgs args = new LoadoutChangeEventArgs() { previous = CurrentLoadout, current = loadout };
        CurrentLoadout = loadout;
        OnLoadoutChanged?.Invoke(this, args);

        // enable new weapon
        if (loadout == LoadoutType.Rifle || loadout == LoadoutType.Pistol || loadout == LoadoutType.SurpressedRifle) {
            CurrentWeapon = weapon;

            CurrentWeapon.Equip();

            for (int i = 0; i < loadoutConfigs.Length; i++) { 
                if (loadoutConfigs[i].loadoutType == loadout) {
                    CurrentWeapon.transform.parent = loadoutConfigs[i].gunPosition;
                    CurrentWeapon.transform.localPosition = Vector3.zero;
                    CurrentWeapon.transform.localRotation = Quaternion.identity;
                    break;
                }
            }
        }

    }

    public bool CanShoot(bool press, bool sprinting) {
        if (CurrentLoadout == LoadoutType.Unarmed) { return false; }

        if (CurrentLoadout == LoadoutType.Rifle || CurrentLoadout == LoadoutType.Pistol || CurrentLoadout == LoadoutType.SurpressedRifle) {

            if (sprinting && CurrentLoadout == LoadoutType.Rifle || sprinting && CurrentLoadout == LoadoutType.SurpressedRifle) {
                return false;
            }
            return CurrentWeapon.CanShoot(press);
        }

        return false;
    }

    public void Shoot(Vector2 target) {
        if (CurrentLoadout == LoadoutType.Unarmed) { return; }

        CurrentWeapon.Shoot(target); 
    }


    [Serializable]
    struct LoadoutConfig {
        public LoadoutType loadoutType;
        public Transform gunPosition;
    }

    public class LoadoutChangeEventArgs : EventArgs {
        public LoadoutType previous;
        public LoadoutType current;
    }

}


[Serializable]
public struct WeaponLoadout
{
    public Weapon weapon;
}
