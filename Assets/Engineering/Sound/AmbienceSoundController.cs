using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSoundController : MonoBehaviour
{
    [SerializeField][Range(0,1)] private float maxVolume = 0.5f;
    [SerializeField][Range(0,3)] private float fadeTime = 1.0f;
    [SerializeField] private AudioSource audioSource;


    Tween t;
    void Start()
    {
        EnterScene();
    }

    public void EnterScene() {
        if (t.IsActive()) {
            t.Kill();
        }
        Tween to = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, maxVolume, fadeTime)
            .OnComplete(() => t = null);
    }

    public void ExitScene() {
        if (t.IsActive()) {
            t.Kill();
        }
        Tween to = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, fadeTime)
            .OnComplete(() => t = null);
    }
}
