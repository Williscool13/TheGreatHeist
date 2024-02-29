using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] Transform crosshair;


    [SerializeField] FlareManager flareManager;
    [SerializeField] PlayerLoadoutManager loadoutManager;

    [Title("Animations")]
    [SerializeField] float timeToFade = 0.55f;

    [Title("UI Elements")]
    [SerializeField] RectTransform flareCooldownRoot;
    [SerializeField] Slider flareCooldown;
    [SerializeField] Image flareImage;
    [SerializeField] RectTransform reloadTimeleftRoot;
    [SerializeField] Slider reloadTimeleft;
    [SerializeField] Image reloadImage;



    [SerializeField] RectTransform crosshairInterface;


    private void Start() {
        flareManager.OnFlareLaunched += OnFlare_DisplayTimeleft;
        flareCooldown.value = 0;
        flareCooldownRoot.localScale = Vector3.one;
        reloadTimeleft.value = 0;
        reloadTimeleftRoot.localScale = Vector3.one;

    }
    void Update() {
        transform.position = crosshair.position;

        FlareCooldown();
        ReloadCooldown();
    }

    void FlareCooldown() {
        if (!flareOnCooldown) { return; }

        if (flareManager.CooldownLeft <= 0) {
            flareOnCooldown = false;
            flareCooldownRoot.DOBlendableScaleBy(Vector3.one * 0.1f, timeToFade);
            Color tarCol = flareImage.color;
            tarCol.a = 0f;
            flareImage.DOColor(tarCol, timeToFade);
            return;
        }

        flareCooldown.value = 1 - (flareManager.CooldownLeft / flareManager.CooldownTime);

    }

    void ReloadCooldown() {
        if (!reloadOnCooldown) { return; }

        /*if (loadoutManager.CurrentWeapon.ReloadTimeLeft <= 0) {
            reloadOnCooldown = false;
            return;
        }*/

        //reloadTimeleft.value = 1 - (loadoutManager.CurrentWeapon.ReloadTimeLeft / loadoutManager.CurrentWeapon.ReloadTime);
    }

    bool flareOnCooldown;
    public void OnFlare_DisplayTimeleft(object o, EventArgs e) {
        flareOnCooldown = true;
        flareCooldownRoot.DOKill();
        flareImage.DOKill();

        flareCooldownRoot.localScale = Vector3.one;
        Color c = flareImage.color;
        c.a = 0.75f;
        flareImage.color = c;


    }

    bool reloadOnCooldown;
    public void OnReload_DisplayTimeleft(object o, EventArgs e) {
        reloadOnCooldown = true;
        reloadTimeleftRoot.localScale = Vector3.one;
        Color c = reloadImage.color;
        c.a = 0.75f;
        reloadImage.color = c;
    }
}
