using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class MultiTriggerWall : MonoBehaviour
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

        int cumulativeCount = 0;

        [SerializeField] private int threshold = 4;

        private void Start() {
            basePos = transform.position;
            endPos = basePos + (Vector2)transform.right * extendDistance;
        }

        [Button("Trigger")]
        public void TriggerWall() {
            cumulativeCount++;
            cumulativeCount = Mathf.Clamp(cumulativeCount, 0, int.MaxValue);
            if (cumulativeCount >= threshold) {
                ExtendWall();
            }
            else {
                RetractWall();
            }
        }
        [Button("Reset")]
        public void ResetWall() {
            cumulativeCount--;
            cumulativeCount = Mathf.Clamp(cumulativeCount, 0, int.MaxValue);
            if (cumulativeCount >= threshold) {
                ExtendWall();
            }
            else {
                RetractWall();
            }
        }

        void ExtendWall() {
            transform.DOKill();

            float _t = TimeFromEnd(transform.position, basePos, endPos);
            if (_t < 0.01f) { return; }
            transform.DOMove(endPos, _t).SetEase(Ease.Linear)
                .OnComplete(() => audioSource.PlayOneShot(wallFinishSound))
                .OnComplete(() => extending = false);
            audioSource.clip = wallExtendSound;
            audioSource.Play();
        }
        void RetractWall() {
            transform.DOKill();

            float _t = TimeFromEnd(transform.position, endPos, basePos);
            if (_t < 0.01f) { return; }
            transform.DOMove(basePos, _t).SetEase(Ease.Linear)
                .OnComplete(() => audioSource.PlayOneShot(wallFinishSound))
                .OnComplete(() => extending = false);
            audioSource.clip = wallRetractSound;
            audioSource.Play();
        }


        float TimeFromEnd(Vector2 position, Vector2 source, Vector2 destination) {
            float ratio = Vector2.Distance(position, destination) / extendDistance;
            return ratio * extendTime;
        }

        bool isOdd(int v) {
            return v % 2 != 0;
        }

        enum CurrentTarget
        {
            None,
            Base,
            End
        }
    }



}
