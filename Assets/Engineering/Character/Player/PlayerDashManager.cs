using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashManager : MonoBehaviour
{
    [Title("Properties")]
    [SerializeField] private float dashCooldown = 1.0f;
    float dashTimestamp;

    public bool CanDash() {
        return Time.time > dashTimestamp;
    }

    public void UseDash() {
        dashTimestamp = Time.time + dashCooldown;
    }
}
