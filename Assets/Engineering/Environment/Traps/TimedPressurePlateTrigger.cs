using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class TimedPressurePlateTrigger : MonoBehaviour
    {
        [SerializeField] private TriggerResponse[] responses;
        [SerializeField] private TriggerResponse[] negativeResponse;
        [SerializeField] private float triggerDuration = 5.0f;
        [Title("Sprite")]
        [SerializeField] Sprite unpressed;
        [SerializeField] Sprite pressed;
        [SerializeField] SpriteRenderer spriteRenderer;
        [Title("Audio")]
        [SerializeField] AudioClip click;
        [SerializeField] AudioClip unclick;
        [SerializeField] AudioSource source;
        [SerializeField] AudioClip[] timeTicks;
        [SerializeField] AudioClip timeUp;


        List<GameObject> occupants = new();
        bool _pressed = false;
        private void OnTriggerEnter2D(Collider2D collision) {
            int occCount = occupants.Count;
            if (!occupants.Contains(collision.gameObject)) { occupants.Add(collision.gameObject); }

            if (occCount == 0 && occupants.Count > 0) {
                for (int i = 0; i < responses.Length; i++) {
                    responses[i].Trigger();
                }
            }

            if (!_pressed) {
                spriteRenderer.sprite = pressed;
                source.PlayOneShot(click);
                timer = triggerDuration;
                _pressed = true;
            }
        }

        [Button("TriggerTest")]
        void Trigger() {
            for (int i = 0; i < responses.Length; i++) {
                responses[i].Trigger();
            }

            if (!_pressed) {
                spriteRenderer.sprite = pressed;
                source.PlayOneShot(click);
                timer = triggerDuration;
                _pressed = true;
            }
        }


        private void OnTriggerExit2D(Collider2D collision) {
            if (occupants.Contains(collision.gameObject)) {
                occupants.Remove(collision.gameObject);
            }
        }

        [SerializeField][ReadOnly] float timer = 0.0f;
        private void Update() {
            if (!_pressed) return;
            if (timer > 0.0f) {
                float wasTimer = timer;
                timer -= Time.deltaTime;
                if (wasTimer > 4 && timer <= 4) {
                    source.PlayOneShot(timeTicks[Random.Range(0, timeTicks.Length)]);
                } else if(wasTimer > 3 && timer <= 3) {
                    source.PlayOneShot(timeTicks[Random.Range(0, timeTicks.Length)]);
                } else if(wasTimer > 2 && timer <= 2) {
                    source.PlayOneShot(timeTicks[Random.Range(0, timeTicks.Length)]);
                } else if(wasTimer > 1 && timer <= 1) {
                    source.PlayOneShot(timeTicks[Random.Range(0, timeTicks.Length)]);
                }
                return;
            }

            if (_pressed) {
                spriteRenderer.sprite = unpressed;
                source.PlayOneShot(timeUp);
                for (int i = 0; i < negativeResponse.Length; i++) {
                    negativeResponse[i].NegativeTrigger();
                }

                if (occupants.Count > 0) {
                    spriteRenderer.sprite = pressed;
                    source.PlayOneShot(click);
                    for (int i = 0; i < responses.Length; i++) {
                        responses[i].Trigger();
                    }

                    timer = triggerDuration;
                } else {
                    _pressed = false;
                }
            }


        }

        void UpdateSprite() {
            spriteRenderer.sprite = occupants.Count > 0 ? pressed : unpressed;
        }

        void PlaySound() {
            if (_pressed && occupants.Count <= 0) {
                source.PlayOneShot(unclick);
            }
            else if (!_pressed && occupants.Count > 0) {
                source.PlayOneShot(click);
            }

            _pressed = occupants.Count > 0;
        }
    }

}
