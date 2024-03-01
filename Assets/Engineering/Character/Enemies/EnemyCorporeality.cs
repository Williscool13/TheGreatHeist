using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorporeality : MonoBehaviour
{
    [Title("Components")]
    [SerializeField] private HealthSystem health;

    [Title("Timers")]
    [SerializeField] private float timeToIncorporeal = 5.0f;
    [SerializeField] private float timeToCorporeal = 5.0f;



    [Title("Corporeality")]
    [SerializeField][ReadOnly] bool corporeal = true;
    [SerializeField]
    [ReadOnly] float corporealValue = 1.0f;

    [Title("Auto Corporeality")]
    [SerializeField] private bool autoCorporeality = false;
    [ShowIf("autoCorporeality")]
    [SerializeField]
    private bool randomAutoCorporeality = false;

    [ShowIf("@autoCorporeality && randomAutoCorporeality")]
    [SerializeField] private Vector2 randomAutoCorporealityRange = new Vector2(5, 10);

    [ShowIf("@autoCorporeality && !randomAutoCorporeality")]
    [SerializeField] private float autoCorporealityTime = 5.0f;

    float corpoTimestamp = 0;
    public float CorporealValue => corporealValue;

    IEnumerator corpRoutine;


    public event EventHandler OnTurnIncorporeal;
    public event EventHandler OnTurnCorporeal;
    private void Start() {
        corpoTimestamp = Time.time + autoCorporealityTime;
        health.OnDeath += OnDeath_Disable;
    }
    private void Update() {
        if (!canIncorporate) return;
        if (!autoCorporeality) return;
        
        if (Time.time > corpoTimestamp) {
            if (randomAutoCorporeality) 
            {
                corpoTimestamp = Time.time + UnityEngine.Random.Range(randomAutoCorporealityRange.x, randomAutoCorporealityRange.y);
            }
            else 
            {
                corpoTimestamp = Time.time + autoCorporealityTime;
            }
            if (corporeal) {
                TurnIncorporeal();
                OnTurnIncorporeal?.Invoke(this, EventArgs.Empty);
            }
            else {
                TurnCorporeal();
                OnTurnCorporeal?.Invoke(this, EventArgs.Empty);
            }
        }
    }




    [Button("Turn Incorporeal")]
    public void TurnIncorporeal() {
        if (corpRoutine != null) { Debug.Log("Corp routine already running"); return; }
        corpRoutine = TurnIncorporealRoutine();
        StartCoroutine(corpRoutine);
    }
    [Button("Turn Corporeal")]
    public void TurnCorporeal() {
        if (corpRoutine != null) { Debug.Log("Corp routine already running"); return; }
        corpRoutine = TurnCorporealRoutine();
        StartCoroutine(corpRoutine);
    }

    IEnumerator TurnIncorporealRoutine() {
        while (corporealValue > 0) {
            corporealValue -= Time.deltaTime / timeToIncorporeal;
            if (corporealValue < 0.5f) corporeal = false;
            yield return null;
        }
        corporealValue = 0;
        corpRoutine = null;
    }

    IEnumerator TurnCorporealRoutine() {
        while (corporealValue < 1) {
            corporealValue += Time.deltaTime / timeToCorporeal;
            if (corporealValue > 0.5f) corporeal = true;
            yield return null;
        }
        corporealValue = 1;
        corpRoutine = null;
    }
    
    public void OnDeath_Disable(object o, EventArgs e) {
        StopCoroutine(corpRoutine);
        this.enabled = false;
    }

    public bool IsCorporeal() {
        return corporeal;
    }

    bool canIncorporate = true;

    public void DisableCorporeality () {
        canIncorporate = false;
        if (corpRoutine != null) {
            StopCoroutine(corpRoutine);
            corpRoutine = null;
        }
        corporealValue = 1.0f;
        corporeal = true;
    }

    public void EnableCorporeality() {
        canIncorporate = true;
    }

    public bool IsChangingCorporeality() {
        return corpRoutine != null;
    }
}
