using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleManager : MonoBehaviour
{
    [SerializeField] private float bulletTimeTimescale = 0.1f;

    public static TimescaleManager Instance;

    private void Start() {
        if (Instance != null) {
            Destroy(this.gameObject);
            Debug.Log("Mopre than 1 timescale manager, deleting");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Button("HalfTime")]
    void HalfTime() {
        Time.timeScale = 0.5f;
    }

    [Button("NormalTime")]
    void NormalTime() {
        Time.timeScale = 1.0f;
    }

    [SerializeField] float bulletTimeDuration = 2.0f;
    [Button("BulletTime")]
    void SlowTime() {
        BulletTime(bulletTimeDuration, false);
    }

    Sequence currentBulletTime;
    public void BulletTime(float duration, bool lerpBack) {
        if (currentBulletTime.IsActive()) {
            currentBulletTime.Kill();
        }
        Time.timeScale = bulletTimeTimescale;
        if (lerpBack) {
            currentBulletTime = DOTween.Sequence()
                .AppendInterval(duration / 2.0f)
                .Append(DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1.0f, duration / 2.0f))
                .SetUpdate(true)
                .OnKill(() => currentBulletTime = null);
        } else {
            currentBulletTime = DOTween.Sequence()
                .AppendInterval(duration)
                .AppendCallback(() => Time.timeScale = 1.0f)
                .SetUpdate(true)
                .OnKill(() => currentBulletTime = null);
        }

    }
}
