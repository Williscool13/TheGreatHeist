using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttention : MonoBehaviour
{
    [Title("Attention Display")]
    [SerializeField] private float dummy = 0.0f;

    [Title("Decay Properties")]
    [SerializeField] private float attentionThreshold = 5.0f;
    [SerializeField] private float attentionMultiplier = 1.0f;

    [Title("Decay Rate")]
    [SerializeField] private float decayRate = 1.0f;
    [SerializeField] private float decayDelay = 1.0f;

    public float AttentionValue { get; private set; }

    float lastAttentionIncreaseTime = 0;
    void Update()
    {
        if (Time.time < lastAttentionIncreaseTime) {
            return;
        }

        AttentionValue -= decayRate * Time.deltaTime;
    }


    public void IncreaseAttention(float value) {
        AttentionValue += value * attentionMultiplier;
        lastAttentionIncreaseTime = Time.time + decayDelay;
    }

    public void ResetAttention() {
        AttentionValue = 0;
    }
}
