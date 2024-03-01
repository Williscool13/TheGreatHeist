using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthInterface : MonoBehaviour
{
    [SerializeField] Canvas cv;
    [SerializeField] RectTransform healthParent;
    [SerializeField] Slider mainHealthSlider;
    [SerializeField] Slider lagHealthSlider1;
    [SerializeField] Slider lagHealthSlider2;

    [Title("Lerp Speeds")]
    [SerializeField] float lag1LerpSpeed;
    [SerializeField] float lag2LerpSpeed;
    [SerializeField] float scaleMoveTowardsSpeed = 1.0f;


    [SerializeField] FloatReference healthReference;
    [SerializeField] HealthData healthData;
    
    float curScale = 1.0f;

    private void Start() {
        cv.worldCamera = Camera.main;
    }
    void Update()
    {
        curScale = Mathf.MoveTowards(curScale, healthReference.Value <= 0 ? 1.03f : 1.0f, Time.deltaTime * scaleMoveTowardsSpeed);
        healthParent.localScale  = new Vector3(curScale, curScale, curScale);

        mainHealthSlider.value = healthReference.Value / (float)healthData.maxHealth;
        if (lagHealthSlider1.value > mainHealthSlider.value) {
            lagHealthSlider1.value = Mathf.Lerp(lagHealthSlider1.value, mainHealthSlider.value, lag1LerpSpeed * Time.deltaTime);
            if (Mathf.Approximately(lagHealthSlider1.value, mainHealthSlider.value)) {
                lagHealthSlider1.value = mainHealthSlider.value;
            }
        } else {
            lagHealthSlider1.value = mainHealthSlider.value;
        }

        if (lagHealthSlider2.value > mainHealthSlider.value) {
            lagHealthSlider2.value = Mathf.Lerp(lagHealthSlider2.value, mainHealthSlider.value, lag2LerpSpeed * Time.deltaTime);
            if (Mathf.Approximately(lagHealthSlider2.value, mainHealthSlider.value)) {
                lagHealthSlider2.value = mainHealthSlider.value;
            }
        } else {
            lagHealthSlider2.value = mainHealthSlider.value;
        }
    }
}
