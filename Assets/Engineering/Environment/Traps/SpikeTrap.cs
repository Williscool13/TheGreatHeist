using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Traps
{
    public class SpikeTrap : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float damageCooldown = 2.0f;

        [Title("MovementProperties")]
        [SerializeField] private float extendTime = 1.0f;
        [SerializeField] private float returnTime = 3.0f;
        [SerializeField] private float lingerTime = 1.0f;
        [SerializeField] private Vector2 movement = new Vector2(2.0f, 0.0f);
        Dictionary<int, float> idTimestampDamaged = new Dictionary<int, float>();
        bool trapActive = false;
        Vector2 basePos;

        private void Start() {
            basePos = transform.position;
        }

        public void TriggerSpikeTrap() {
            if (trapActive) return;
            trapActive = true;
            DOTween.Sequence()
                .Append(transform.DOMove(basePos + movement, extendTime).SetEase(Ease.Linear))
                .AppendInterval(lingerTime)
                .Append(transform.DOMove(basePos, returnTime).SetEase(Ease.OutSine))
                .OnComplete(() => trapActive = false);
                
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision == null) return;
            if (idTimestampDamaged.ContainsKey(collision.gameObject.GetInstanceID())) {
                if (idTimestampDamaged[collision.gameObject.GetInstanceID()] > Time.time) {
                    return;
                }
            }
            HealthSystem hs = collision.transform.GetComponentInChildren<HealthSystem>();
            if (hs == null) return;

            idTimestampDamaged[collision.gameObject.GetInstanceID()] = Time.time + damageCooldown;
            hs.OnHit(new DamageData(damage, transform.position, transform.position - collision.transform.position, 1.0f, false, Time.time, DamageImpactType.Sharp));
        }

    }
}
