using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorporeality : MonoBehaviour
{

    [SerializeField] private float timeToIncorporeal = 5.0f;
    [SerializeField] private float timeToCorporeal = 5.0f;

    [SerializeField][ReadOnly] bool corporeal = true;
    [SerializeField]
    [ReadOnly] float corporealValue = 1.0f;
    public float CorporealValue { get; private set; }

    IEnumerator corpRoutine;
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
        corpRoutine = null;
    }

    IEnumerator TurnCorporealRoutine() {
        while (corporealValue < 1) {
            corporealValue += Time.deltaTime / timeToCorporeal;
            if (corporealValue > 0.5f) corporeal = true;
            yield return null;
        }
        corpRoutine = null;
    }
    

    public bool IsCorporeal() {
        return corporeal;
    }
}
