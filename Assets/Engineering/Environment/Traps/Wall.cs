using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Traps
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private float extendTime = 1.0f;
        [SerializeField] private float extendDistance = 2.0f;
        Vector2 basePos;
        Vector2 endPos;

        [Title("Sound")]
        [SerializeField] private AudioClip wallExtendSound;
        [SerializeField] private AudioClip wallRetractSound;
        [SerializeField] private AudioClip wallFinishSound;
        [SerializeField] private AudioSource audioSource;
        bool extending = false;
        private void Start() {
            basePos = transform.position;
            endPos = basePos + (Vector2)transform.right * extendDistance;
        }

        public void TriggerWall() {
            if (extending) return;
            float _t = TimeFromEnd(transform.position, basePos, endPos);
            if (_t <= 0) {
                Debug.Log("already at dest");
                return;
            }
            transform.DOMove(endPos, _t).SetEase(Ease.Linear)
                .OnComplete(() => audioSource.PlayOneShot(wallFinishSound))
                .OnComplete(() => extending = false);
            audioSource.PlayOneShot(wallExtendSound);
        }

        public void ResetWall() {
            float _t = TimeFromEnd(transform.position, endPos, basePos);
            if (_t <= 0) {
                Debug.Log("already at base");
                return;
            }
            transform.DOMove(basePos, _t).SetEase(Ease.Linear)
                .OnComplete(() => audioSource.PlayOneShot(wallFinishSound));
            audioSource.PlayOneShot(wallRetractSound);
        }

        float TimeFromEnd(Vector2 position, Vector2 source, Vector2 destination) {
            float ratio = Vector2.Distance(source, destination) / extendDistance;
            return ratio * extendTime;
        }

        enum CurrentTarget {
            None,
            Base,
            End
        }
    }

    

}
