using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BackupPower : MonoBehaviour
{
    [SerializeField] private Light2D[] lights;
    public void EnableBackupPower() {
        foreach (Light2D _l in lights) {
            DOTween.To(() => _l.intensity, x => _l.intensity = x, 1, 3.0f);
        }
    }
}