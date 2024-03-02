using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerBox : MonoBehaviour, ITarget, IInteractable
{

    [SerializeField] float maxHealth = 100;
    [SerializeField] private NullEvent mainPowerShutoffEvent;
    [SerializeField] private NullEvent backupPowerEnabledEvent;
    [SerializeField] private BoolVariable mainPowerShutoff;
    [SerializeField][ReadOnly] float currHealth = 100;
    [SerializeField][Range(0, 1.5f)] private float lightsOutDelay = 0.5f;

    [Title("Sound")]
    [SerializeField] private AudioClip powerBoxHitSound;
    [SerializeField] private AudioClip[] powerBoxDestroyedSounds;
    [SerializeField] private AudioClip backupPowerupSound;
    [SerializeField] private AudioSource audioSource;

    [Title("Effects")]
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private ParticleSystem hitParticles;

    [SerializeField] Light2D mainLight;

    [Title("Interaction")]
    [SerializeField] private bool interactionEnabled = true;

    bool disabled = false;
    public bool CanInteract => interactionEnabled && !disabled;
    public void OnHit(DamageData data) {
        if (disabled) { return; }
        currHealth -= data.amount;
        audioSource.PlayOneShot(powerBoxHitSound);
        if (currHealth <= 0) {
            foreach (var powerBoxDestroyedSound in powerBoxDestroyedSounds) {
                audioSource.PlayOneShot(powerBoxDestroyedSound);
            }
            destroyedParticles.Play();
            mainPowerShutoffEvent.Raise(null);
            DOTween.Sequence()
                .AppendInterval(lightsOutDelay)
                .Append(DOTween.To(() => mainLight.intensity, x => mainLight.intensity = x, 0, 1.5f))
                .AppendInterval(1.5f - lightsOutDelay)
                .Play()
                .onComplete += () => { backupPowerEnabledEvent.Raise(null); audioSource.PlayOneShot(backupPowerupSound); };

            mainPowerShutoff.Value = true;
            disabled = true;
            Destroy(this);
        } else {
            hitParticles.Play();
        }
    }

    public void Interact() {
        if (interactionEnabled && !disabled) {
            Explode();
            Unhighlight();
        }
    }

    [Button("Explode")]
    public void Explode() {
        OnHit(new DamageData(100, Vector3.zero, Vector3.zero, 10.0f, true, Time.time, DamageImpactType.Sharp));
    }

    void Start() { 
        currHealth = maxHealth;
        mainPowerShutoff.Value = false;
    }

    public void Highlight() {
        Debug.Log("HIGHLIGHTING HIGHLIGHTING HIGHLIGHTING");
    }

    public void Unhighlight() {
        Debug.Log("UNHIGHLIGHT UNHIGHLIGHT UNHIGHLIGHT");
    }
}
