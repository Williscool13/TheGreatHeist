using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{

    [SerializeField] CharacterMovement movement;
    [SerializeField] PlayerLoadoutManager loadoutManager;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite pistolSprite;
    [SerializeField] Sprite rifleSprite;
    [SerializeField] Sprite sprintRifleSprite;
    [SerializeField] Sprite unarmedSprite;

    private void Awake() {
        //movement.OnMovementTypeChanged += Movement_OnMovementTypeChanged;
        //loadoutManager.OnLoadoutChanged += LoadoutManager_OnLoadoutChanged;
    }

    private void LoadoutManager_OnLoadoutChanged(object sender, PlayerLoadoutManager.LoadoutChangeEventArgs e) {

        /*SpriteUpdate(e.current, movement.CurrentMovementType);
        Debug.Log("[Loadout] Sprite updated to: " + spriteRenderer.sprite.name);
        Debug.Log("[Loadout] Movement type: " + movement.CurrentMovementType);
        Debug.Log("[Loadout] Loadout type: " + e.current);*/
    }

    private void Movement_OnMovementTypeChanged(object sender, CharacterMovement.MovementChangeEventArgs e) {
        /*SpriteUpdate(loadoutManager.CurrentLoadout, e.current);
        Debug.Log("[Movement] Sprite updated to: " + spriteRenderer.sprite.name);
        Debug.Log("[Movement] Movement type: " + e.current);
        Debug.Log("[Movement] Loadout type: " + loadoutManager.CurrentLoadout);*/
    }


    void SpriteUpdate(LoadoutType l, CharacterMovement.Movement m) {
        /*switch (l) {
            case PlayerLoadoutManager.LoadoutType.Pistol:
                spriteRenderer.sprite = pistolSprite;
                break;
            case PlayerLoadoutManager.LoadoutType.Rifle:
                if (m == CharacterMovement.Movement.Sprinting) {
                    spriteRenderer.sprite = sprintRifleSprite;
                }
                else {
                    spriteRenderer.sprite = rifleSprite;
                }
                break;
            case PlayerLoadoutManager.LoadoutType.SurpressedRifle:
                Debug.LogError("Surpressed Rifle not implemented (And is not planned to be)");
                break;
            case PlayerLoadoutManager.LoadoutType.Unarmed:
                spriteRenderer.sprite = unarmedSprite;
                break;
        }*/
    }
}
