using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Traps;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer sr;

    [Title("Properties")]
    [SerializeField] private bool repeatable = false;
    [ShowIf("repeatable")][SerializeField] private float repeatDelay = 0.5f;
    [ShowIf("repeatable")][SerializeField] private bool negativeTriggerOnReset = false;
    [SerializeField] private float offsetDistance = 0.1f;
    [SerializeField] private float offsetDuration = 0.5f;

    [Title("Sound")]
    [SerializeField] private AudioClip buttonPressSound;
    [SerializeField] private AudioClip buttonReleaseSound;
    [SerializeField] private AudioSource audioSource;

    [Title("Triggers")]
    [SerializeField] private TriggerResponse[] triggers;


    bool activated;


    public bool CanInteract => !activated;

    public void Highlight() {
        sr.material.SetFloat("_Thickness", 8.0f);
    }
    [Button("Test Interact")]
    public void Interact() {
        if (activated) {
            Debug.Log("Shouldnt be able to do this");
            return; 
        }

        if (!repeatable) {
            // move button once
            audioSource.PlayOneShot(buttonPressSound);
            transform.DOMove((Vector2)transform.position + (Vector2)(-transform.up * offsetDistance), offsetDuration).SetEase(Ease.InSine)
                .OnComplete(() => 
                {
                    for (int i = 0; i < triggers.Length; i++) { triggers[i].Trigger(); }
                });
        }
        else {
            DOTween.Sequence()
                .AppendCallback(() => audioSource.PlayOneShot(buttonPressSound))
                .Append(transform.DOBlendableMoveBy((Vector2)(-transform.up * offsetDistance), offsetDuration).SetEase(Ease.InSine))
                .AppendCallback(() => {
                    for (int i = 0; i < triggers.Length; i++) { triggers[i].Trigger(); }
                })
                .AppendInterval(repeatDelay)
                .AppendCallback(() => audioSource.PlayOneShot(buttonReleaseSound))
                .Append(transform.DOBlendableMoveBy((Vector2)(transform.up * offsetDistance), offsetDuration).SetEase(Ease.Linear))
                .AppendCallback(() => {
                    if (negativeTriggerOnReset) {
                        for (int i = 0; i < triggers.Length; i++) { triggers[i].NegativeTrigger(); }
                    }
                })  
                .OnComplete(() => activated = false);
            // move button with yoyo 
        }

        activated = true;
        Debug.Log("Press");
        Unhighlight();
    }

    public void Unhighlight() {
        sr.material.SetFloat("_Thickness", 0.0f);
    }
}
