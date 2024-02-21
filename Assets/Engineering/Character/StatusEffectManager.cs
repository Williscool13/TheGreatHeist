using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    [field: SerializeField]
    [field: ReadOnly]
    public bool Stunned { get; private set; }
    float stunTimer;

    public bool Invulnerable { get; private set; }

    [AssetsOnly][SerializeField] GameObject laughParticleEffect;
    ParticleSystem laughParticle;


    private void Start() {
        laughParticle = Instantiate(laughParticleEffect, transform).GetComponent<ParticleSystem>();
        laughParticle.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        laughParticle.Stop();
        laughParticle.Clear();
    }

    void Update()
    {
        if (!Stunned) return;
        
        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0) {
            UnStun();
        }
    }


    public void StartStunTimer(float duration) {
        if (duration > stunTimer) {
            stunTimer = duration;
            Stun();
        }

    }



    public void EndStunTimer() {
        stunTimer = 0;
    }


    public void SetInvulnerable(bool invulnerable) {
        Invulnerable = invulnerable;
    }


    void Stun() {
        Stunned = true;
        laughParticle.Play();
    }

    void UnStun() {
        Stunned = false;
        laughParticle.Stop();
    }



#if UNITY_EDITOR
    [Button("Stun For 100 Seconds")]
    public void StunTest() {
        StartStunTimer(100);
    }
#endif
}
