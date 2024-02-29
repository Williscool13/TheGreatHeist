using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;



public class LightFlare : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Light2D light2D;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float activationDelay = 0.5f;
    float targetIntensity;
    float lifetimeTimer = 0;
    bool active;
    bool shrinking;

    public bool Active { get => active; }

    ObjectPool<LightFlare> ownerFlarePool;
    public event EventHandler<LightFlare> OnFlareRelease;
    public void Initialize(Vector2 position, float intensity, float radius, float lifetime, ObjectPool<LightFlare> owner) {
        // set light radius, and other things back to default
        transform.position = position;
        light2D.intensity = 0.1f;
        targetIntensity = intensity;
        light2D.pointLightOuterRadius = radius;
        lifetimeTimer = lifetime;
        audioSource.volume = 0;

        ownerFlarePool = owner;
        active = false;
        shrinking = false;
    }

    public void Launch(Vector2 velocity, float torque) {
        rb.AddTorque(torque);
        rb.AddForce(velocity, ForceMode2D.Impulse);

        DOTween.Sequence()
            .AppendInterval(activationDelay)
            .AppendCallback(() => {
                DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetIntensity, 1f);
                DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0.5f, 1f);
                active = true;
            });
    }

    private void Update() {
        if (!shrinking && lifetimeTimer < 1) {
            shrinking = true;
            DOTween.To(() => light2D.intensity, x => light2D.intensity = x, 0, lifetimeTimer);
            DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, lifetimeTimer);
        }
        if (active) {
            lifetimeTimer -= Time.deltaTime;
            if (lifetimeTimer <= 0) {
                OnFlareRelease?.Invoke(this, this);
                ownerFlarePool.Release(this);
                active = false;
            }
        }
    }
}