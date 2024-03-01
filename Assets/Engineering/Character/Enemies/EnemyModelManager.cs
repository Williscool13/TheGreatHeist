using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyModelManager : MonoBehaviour
{

    [SerializeField] private EnemyCorporeality corporeality;
    [SerializeField] private SpriteRenderer enemyModel;
    [SerializeField] private Light2D flashlight;
    [SerializeField] private EnemyAttentionMark attentionMark;
    [SerializeField] private Collider2D hitbox;


    bool watchingCorporeality = false;
    float corporealityTarget = 0.0f;
    float targetFlashlightIntensity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        corporeality.OnTurnIncorporeal += OnTurnIncorporeal_WatchCorporealityValue;
        corporeality.OnTurnCorporeal += OnTurnCorporeal_WatchCorporealityValue;
        targetFlashlightIntensity = flashlight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!watchingCorporeality) return;

        if (!corporeality.IsChangingCorporeality()) {
            Debug.Log($"Corporeal Value Reached (or at least, no ongoing changes): {corporeality.CorporealValue} and {corporealityTarget}");
            watchingCorporeality = false;

            Color _c = enemyModel.color;
            _c.a = corporeality.CorporealValue;
            enemyModel.color = _c;
            attentionMark.TargetAlpha = corporeality.CorporealValue;
            flashlight.intensity = targetFlashlightIntensity * corporeality.CorporealValue;
            return;
        }
        
        if (Mathf.Approximately(0.0f, corporealityTarget)) {
            if (corporeality.CorporealValue <= 0.5f) {
                hitbox.enabled = false;
            }
        } else if (Mathf.Approximately(1.0f, corporealityTarget)) {
            if (corporeality.CorporealValue >= 0.5f) {
                hitbox.enabled = true;
            }
        }

        Color c = enemyModel.color;
        c.a = corporeality.CorporealValue;
        enemyModel.color = c;
        attentionMark.TargetAlpha = corporeality.CorporealValue;
        flashlight.intensity = targetFlashlightIntensity * corporeality.CorporealValue;
    }

    public void OnTurnIncorporeal_WatchCorporealityValue(object o, EventArgs e) {
        watchingCorporeality = true;
        corporealityTarget = 0.0f;
    }

    public void OnTurnCorporeal_WatchCorporealityValue(object o, EventArgs e) {
        watchingCorporeality = true;
        corporealityTarget = 1.0f;
    }

    public void BackupPowerEnabled() {
        targetFlashlightIntensity = 1.0f;
    }
}
