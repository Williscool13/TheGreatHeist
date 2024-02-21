using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    [SerializeField] float aimSpeedMultiplier = 1;
    [SerializeField] float combatAimRotationSpeed = 720;
    [SerializeField] float alertAimRotationTime = 1.5f;
    [SerializeField] float unalertAimRotationTime = 3.0f;
    public float AimSpeedMultiplier { get => aimSpeedMultiplier; set => aimSpeedMultiplier = value; }
    public int CurrentSweepCount { get => currSweepCount; set => currSweepCount = value; }
    public bool Sweeping => sweeping;


    int currSweepCount = 0;
    bool sweeping = false;

    bool rotateTweening = false;
    public bool TweenRotate(Vector2 direction, float time) {
        if (rotateTweening) return false;
        transform.DORotate(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg), time / aimSpeedMultiplier)
            .onComplete += () => rotateTweening = false;
        rotateTweening = true;
        return true;
    }

    public void StopAllAim() {
        rotateTweening = false;
        sweeping = false;
        int c = DOTween.Kill(transform);
        Debug.Log("Killed " + c + " tweens");
    }


    public void RotateTowards(Vector2 direction) {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg), combatAimRotationSpeed * Time.deltaTime);
    }

    public float Sweep(bool alert) {
        if (sweeping) return 0;
        sweeping = true;
        float time = alert ? alertAimRotationTime : unalertAimRotationTime;
        transform.DOBlendableRotateBy(new Vector3(0, 0, -180), time)
            .onComplete += () => { sweeping = false; currSweepCount++; };
        return time;
    }

    public float RandomSweep(bool alert) {
        if (sweeping) return 0;
        sweeping = true;
        float v = Random.Range(90, 180);
        float d = Random.value > 0.5f ? 1 : -1;
        float time = alert ? alertAimRotationTime : unalertAimRotationTime;
        transform.DOBlendableRotateBy(new Vector3(0, 0, v * d), time)
            .onComplete += () => { sweeping = false; currSweepCount++; };
        return time;
    }

    public Vector2 GetAimDirection() { 
        return transform.right;
    }
}
