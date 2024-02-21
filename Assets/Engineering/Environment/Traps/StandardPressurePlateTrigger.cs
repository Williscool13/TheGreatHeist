using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class StandardPressurePlateTrigger : MonoBehaviour
    {
        [SerializeField] private TriggerResponse[] responses;
        [Title("Sprite")]
        [SerializeField] Sprite unpressed;
        [SerializeField] Sprite pressed;
        [SerializeField] SpriteRenderer spriteRenderer;
        [Title("Audio")]
        [SerializeField] AudioClip click;
        [SerializeField] AudioClip unclick;
        [SerializeField] AudioSource source;


        List<GameObject> occupants = new();
        bool _pressed = false;
        private void OnTriggerEnter2D(Collider2D collision) {
            int occCount = occupants.Count;
            if (!occupants.Contains(collision.gameObject)) {  occupants.Add(collision.gameObject); }

            if (occCount == 0) {
                for (int i = 0; i < responses.Length; i++) {
                    responses[i].Trigger();
                }
            }

            UpdateSprite();
            PlaySound();
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (occupants.Contains(collision.gameObject)) {
                occupants.Remove(collision.gameObject);
            }
            UpdateSprite();
            PlaySound();
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
