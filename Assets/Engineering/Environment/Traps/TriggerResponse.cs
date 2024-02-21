using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Traps
{
    public class TriggerResponse : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggerEvent;
        [SerializeField] private UnityEvent negativeTriggerEvent;

        public void Trigger() {
            triggerEvent.Invoke();
        }

        public void NegativeTrigger() {
            negativeTriggerEvent.Invoke();
        }

    }

}
