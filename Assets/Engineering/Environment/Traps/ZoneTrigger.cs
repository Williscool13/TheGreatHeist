using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Traps;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{

    [SerializeField] private bool repeatable = false;
    [SerializeField] private TriggerType triggerType;
    [Title("Triggers")]
    [SerializeField] private TriggerResponse[] triggers;

    bool triggered;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.CompareTag("Player") || triggered) { return; }

        triggered = true;

        for (int i = 0; i < triggers.Length; i++) { 
            switch (triggerType) {
                case TriggerType.Positive:
                    triggers[i].Trigger();
                    break;
                case TriggerType.Negative:
                    triggers[i].NegativeTrigger();
                    break;
            }
        }

        Debug.Log("Zone Trigger - Triggered");
    }

    enum TriggerType {
        Positive,
        Negative
    }
}
