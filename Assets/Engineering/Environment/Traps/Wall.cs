using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Traps
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private float extendTime = 1.0f;
        [SerializeField] private Vector2 extendVector = new Vector2(0.0f, 2.0f);
        Vector2 basePos;
        Vector2 endPos;

        private void Start() {
            basePos = transform.position;
            endPos = basePos + extendVector;
        }

        public void TriggerWall() {
            float _t = TimeFromEnd(transform.position, basePos, endPos);
            if (_t <= 0) {
                Debug.Log("already at dest");
                return;
            }
            transform.DOMove(endPos, _t).SetEase(Ease.Linear);
        }

        public void ResetWall() {
            float _t = TimeFromEnd(transform.position, endPos, basePos);
            if (_t <= 0) {
                Debug.Log("already at base");
                return;
            }
            transform.DOMove(basePos, _t).SetEase(Ease.Linear);
        }

        float TimeFromEnd(Vector2 position, Vector2 source, Vector2 destination) {
            float ratio = Vector2.Distance(source, destination) / extendVector.magnitude;
            return ratio * extendTime;
        }

        enum CurrentTarget {
            None,
            Base,
            End
        }
    }

    

}
