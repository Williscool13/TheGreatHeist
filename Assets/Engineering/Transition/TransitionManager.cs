using DG.Tweening;
using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Material circleWipeMaterial;
    [SerializeField] private float transitionDuration;

    [SerializeField] private NullEvent transitionStartEvent;
    [SerializeField] private NullEvent transitionLingerEvent;
    [SerializeField] private NullEvent transitionEndEvent;

    public event EventHandler transitionStart;
    public event EventHandler transitionLinger;
    public event EventHandler transitionEnd;
    

    public static TransitionManager Instance { get; private set; }

    Sequence currentTransition = null;

    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            Debug.Log("more than 1 transition manager");
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        circleWipeMaterial.SetFloat("_Radius", 1.5f);

    }
    public void Transition(float initialDelay, float lingerTime, Transform target) {
        //Vector2 baseOffset = Vector2.one * 0.5f;
        currentTransition = DOTween.Sequence()
            .AppendInterval(initialDelay)
            .AppendCallback(() => {
                Vector2 offset = Camera.main.WorldToViewportPoint(target.position);
                circleWipeMaterial.SetVector("_Offset", offset);
                transitionStartEvent.Raise(null);
                transitionStart?.Invoke(this, EventArgs.Empty);
                })
            .Append(DOTween.To(() => circleWipeMaterial.GetFloat("_Radius"), x => circleWipeMaterial.SetFloat("_Radius", x), 0, transitionDuration))
            .AppendCallback(() => {
                transitionLingerEvent.Raise(null);
                transitionLinger?.Invoke(this, EventArgs.Empty);
                circleWipeMaterial.SetVector("_Offset", Vector2.one * 0.5f);
            })
            .AppendInterval(lingerTime)
            .Append(DOTween.To(() => circleWipeMaterial.GetFloat("_Radius"), x => circleWipeMaterial.SetFloat("_Radius", x), 1.5f, transitionDuration))
            .AppendCallback(() => { 
                transitionEndEvent.Raise(null); 
                transitionEnd?.Invoke(this, EventArgs.Empty);
            })
            .OnKill(() => currentTransition = null);
    }

    public bool IsTransitioning() {
        return currentTransition.IsActive();
    }
}
