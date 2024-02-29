using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FlareManager : MonoBehaviour
{

    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private Transform flareLaunchTransform;

    [Title("Components")]
    [SerializeField] private PlayerAim aim;

    [Title("Flare Properties")]
    [SerializeField] private int maxFlareCount = 3;
    [SerializeField] private float flareForce = 10f;
    [SerializeField] private float flareTorque = 5f;

    [Title("Cooldown")]
    [SerializeField] private bool cooldownEnabled = false;
    [ShowIf("cooldownEnabled")]
    [SerializeField] private float cooldownTime = 1f;

    [Title("Sound")]
    [SerializeField] private AudioSource flareSound;
    [SerializeField] private AudioClip flareLaunchSound;

    ObjectPool<LightFlare> flarePool;
    [SerializeField][ReadOnly] int currentFlareCount = 0;


    float currentFlareTimestamp = 0;

    public float CooldownLeft { get { return currentFlareTimestamp - Time.time; } }
    public float CooldownTime { get { return cooldownTime; } }
    public event EventHandler OnFlareLaunched;

    private void Start() {
        flarePool = new ObjectPool<LightFlare>(FlareCreate, FlareOnTakeFromPool, FlareOnReleaseToPool, FlareOndestroyFromPool, true, 10, 60);
    }

    public bool CanFlare(bool press) {
        if (!press) { return false; }

        if (currentFlareCount >= maxFlareCount) { return false; }

        if (cooldownEnabled && Time.time < currentFlareTimestamp) { return false; }

        return true;
    }

    public void Flare() {
        Debug.Log("FLARE LAUNCHED");

        //LightFlare f = flarePool.Get();
        LightFlare f = flarePool.Get();
        f.Initialize(flareLaunchTransform.position, 1, 10, 10, flarePool);
        Vector2 m = aim.GetMousePositionWorld();
        Vector2 dir = m - (Vector2)flareLaunchTransform.position;
        f.Launch(dir.normalized * (flareForce + UnityEngine.Random.Range(-2.0f, 2.0f)), Mathf.Sign(UnityEngine.Random.value) * (flareTorque + UnityEngine.Random.Range(-2.0f, 2.0f)));

        currentFlareTimestamp = Time.time + cooldownTime;
        OnFlareLaunched?.Invoke(this, EventArgs.Empty);

        flareSound.PlayOneShot(flareLaunchSound);


    }


    #region Pooling
    LightFlare FlareCreate() {
        GameObject flare = Instantiate(flarePrefab, Vector3.one * -100f, Quaternion.identity);
        LightFlare lf = flare.GetComponent<LightFlare>();

        return lf;
    }

    void FlareOnTakeFromPool(LightFlare lightFlare) {
        lightFlare.gameObject.SetActive(true);
        currentFlareCount++;
    }

    void FlareOnReleaseToPool(LightFlare lightFlare) {
        lightFlare.gameObject.SetActive(false);
        currentFlareCount--;
    }

    void FlareOndestroyFromPool(LightFlare lightFlare) {
        Destroy(lightFlare.gameObject);
    }
    #endregion
}