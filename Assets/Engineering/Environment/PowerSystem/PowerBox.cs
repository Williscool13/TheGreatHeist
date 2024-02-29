using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerBox : MonoBehaviour, ITarget
{

    [SerializeField] float maxHealth = 100;
    [SerializeField] private NullEvent mainPowerShutoffEvent;
    [SerializeField] private NullEvent backupPowerEnabledEvent;
    [SerializeField][ReadOnly] float currHealth = 100;

    [Title("Sound")]
    [SerializeField] private AudioClip powerBoxHitSound;
    [SerializeField] private AudioClip[] powerBoxDestroyedSounds;
    [SerializeField] private AudioClip backupPowerupSound;
    [SerializeField] private AudioSource audioSource;

    [Title("Effects")]
    [SerializeField] private ParticleSystem destroyedParticles;
    [SerializeField] private ParticleSystem hitParticles;

    [SerializeField] Light2D mainLight;
    public void OnHit(DamageData data) {
        currHealth -= data.amount;
        audioSource.PlayOneShot(powerBoxHitSound);
        if (currHealth <= 0) {
            foreach (var powerBoxDestroyedSound in powerBoxDestroyedSounds) {
                audioSource.PlayOneShot(powerBoxDestroyedSound);
            }
            destroyedParticles.Play();
            mainPowerShutoffEvent.Raise(null);
            DOTween.Sequence()
                .AppendInterval(1.0f)
                .Append(DOTween.To(() => mainLight.intensity, x => mainLight.intensity = x, 0, 1.5f))
                .Play()
                .onComplete += () => { backupPowerEnabledEvent.Raise(null); audioSource.PlayOneShot(backupPowerupSound); };
                

            Destroy(this);
        } else {
            hitParticles.Play();
        }
    }

    [Button("Explode")]
    public void Explode() {
        OnHit(new DamageData(100, Vector3.zero, Vector3.zero, 10.0f, true, Time.time, DamageImpactType.Sharp));
    }

    void Start() { currHealth = maxHealth; }
}
