using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLoadoutManager : MonoBehaviour
{
    public LoadoutType CurrentLoadout { get; private set; } = LoadoutType.Unarmed;
    public Weapon CurrentWeapon { get; private set; } = null;


    [SerializeField] LoadoutConfig[] loadoutConfigs;


    [SerializeField] LoadoutType startingLoadout;
    [SerializeField][SceneObjectsOnly] Weapon startingWeapon;


    public event EventHandler<LoadoutChangeEventArgs> OnLoadoutChanged;

    void Start()
    {
        ChangeLoadout(startingLoadout, startingWeapon);
    }

    public void ChangeLoadout(LoadoutType loadout, Weapon weapon) {

        // drop current weapon (i.e. spawn weapon pickup and set its remaining ammo to current ammo)
        /*if (CurrentWeapon != null) {
            CurrentWeapon.gameObject.SetActive(false);
        }
        CurrentWeapon.Unequip();
         */
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

        if (CurrentLoadout == LoadoutType.Rifle || CurrentLoadout == LoadoutType.Pistol || CurrentLoadout == LoadoutType.SurpressedRifle) {

            if (sprinting && CurrentLoadout == LoadoutType.Rifle || sprinting && CurrentLoadout == LoadoutType.SurpressedRifle) {
                return false;
            }
            return CurrentWeapon.CanShoot(press);
        }

        return false;
    }

    public void Shoot(Vector2 target) {
        CurrentWeapon.Shoot(target); 
    }

    public enum LoadoutType {
        Rifle,
        Pistol,
        SurpressedRifle,
        Carryable,
        Unarmed
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
