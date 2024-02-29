using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttention : MonoBehaviour
{
    [Title("Attention Display")]
    [SerializeField][ReadOnly] private float attentionValue = 0;


    [Title("Attention Properties")]
    [SerializeField] private float attentionThreshold = 5.0f;
    [SerializeField] private float attentionMultiplier = 1.0f;

    [Title("Decay Rate")]
    [SerializeField] private float decayMultiplier = 1.0f;
    [SerializeField] private float decayDelay = 1.0f;

    [Title("Sound")]
    [SerializeField] private float alertSoundCooldown = 1.0f;
    [SerializeField] private float investigateSoundCooldown = 1.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] alertSounds;
    [SerializeField] private AudioClip[] investigateSounds;

    [Title("Corporeality")]
    [SerializeField] private bool requiresCorporeality = false;
    [ShowIf("requiresCorporeality")]
    [SerializeField] private EnemyCorporeality corporeality;

    [Title("Color")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color alertedColor = Color.red;
    [SerializeField] private Color investigateColor = Color.yellow;
    [SerializeField] private Color idleColor = Color.green;

    public event EventHandler<AttentionInvestigateEventArgs> OnAttentionInvestigate;
    public event EventHandler<AttentionAlertEventArgs> OnAttentionAlert;

    float lastAttentionIncreaseTime = 0;
    float alertSoundTimestamp = 0;
    float investigateSoundTimestamp = 0;

    public float AttentionValue { get => attentionValue; }
    public float AttentionThreshold { get => attentionThreshold; }


    private void Update() {
        if (attentionValue <= 0) {
            sprite.material.SetColor("_Outline_Color", idleColor);
        } else {
            Color tarCol = Color.Lerp(investigateColor, alertedColor, attentionValue / attentionThreshold);
            sprite.material.SetColor("_Outline_Color", tarCol);
        }
    }

    public void SetSpriteOutlineColor(Color col) {
        sprite.material.SetColor("_Outline_Color", col);
    }

    public bool IsAttentionAlerted() {
        return attentionValue >= attentionThreshold;
    }

    /// <summary>
    /// Called if the enemy is alerted by a trigger but not strictly by the player
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetPosition"></param>
    public void IncreaseAttention(float value, Vector2 targetPosition) {
        attentionValue += value * attentionMultiplier;
        attentionValue = Mathf.Min(attentionValue, attentionThreshold / 2.0f);
        lastAttentionIncreaseTime = Time.time + decayDelay;

        OnAttentionInvestigate?.Invoke(this, new AttentionInvestigateEventArgs(targetPosition));
        if (Time.time > investigateSoundTimestamp) {
            investigateSoundTimestamp = Time.time + investigateSoundCooldown;
            audioSource.PlayOneShot(investigateSounds[UnityEngine.Random.Range(0, investigateSounds.Length)]);
        }
    }

    /// <summary>
    /// Called if an enemy is alerted by the player
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetPosition"></param>
    /// <param name="target"></param>
    /// <param name="hitbox"></param>
    public void IncreaseAttention(float value, Vector2 targetPosition, Transform target, IHitbox hitbox) {
        if (requiresCorporeality && !corporeality.IsCorporeal()) {
            return;
        }

        attentionValue += value * attentionMultiplier;
        lastAttentionIncreaseTime = Time.time + decayDelay;

        if (IsAttentionAlerted()) {
            attentionValue = attentionThreshold * 1.25f;
            OnAttentionAlert?.Invoke(this, new AttentionAlertEventArgs(targetPosition, target, hitbox));
            if (Time.time > alertSoundTimestamp) {
                alertSoundTimestamp = Time.time + alertSoundCooldown;
                audioSource.PlayOneShot(alertSounds[UnityEngine.Random.Range(0, alertSounds.Length)]);
            }
        }
    }


    public void DecrementAttention(float value) {
        if (requiresCorporeality && !corporeality.IsCorporeal()) {
            return;
        }

        if (Time.time < lastAttentionIncreaseTime || attentionValue <= 0.0f) {
            return;
        }
        attentionValue -= value * decayMultiplier;
        attentionValue = Mathf.Max(0, attentionValue);
    }


    public void ResetAttention() {
        attentionValue = 0;
    }

}

public class AttentionInvestigateEventArgs { 
    public Vector2 targetPosition;
    public AttentionInvestigateEventArgs(Vector2 targetPosition) {
        this.targetPosition = targetPosition;
    }
}

public class AttentionAlertEventArgs
{
    public Vector2 targetPosition;

    public Transform target;
    public IHitbox hitbox;
    public AttentionAlertEventArgs(Vector2 targetPosition, Transform target, IHitbox hitbox) {
        this.targetPosition = targetPosition;
        this.target = target;
        this.hitbox = hitbox;
    }
}