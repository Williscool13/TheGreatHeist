using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttentionMark : MonoBehaviour
{
    [Title("Follow Target")]
    [SerializeField] Collider2D targetHitbox;
    [SerializeField] float heightOffset = 1.3f;
    [SerializeField] float lerpSpeed = 0.1f;

    [Title("Color")]
    [SerializeField] EnemyAttention attention;
    [SerializeField] Color alertedColor = Color.red;
    [SerializeField] Color investigateColor = Color.yellow;

    [SerializeField] SpriteRenderer spriteRenderer;


    void Update()
    {
        float targetYOffset = targetHitbox.bounds.extents.y * heightOffset;
        float lerpY = Mathf.Lerp(transform.localPosition.y, targetYOffset, lerpSpeed);
        float lerpX = Mathf.Lerp(transform.localPosition.x, 0, lerpSpeed);
        
        transform.position = targetHitbox.transform.position + new Vector3(lerpX, lerpY, 0);
        transform.rotation = Quaternion.identity;

        if (attention.AttentionValue <= 0) {
            spriteRenderer.enabled = false;
        } else {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.Lerp(investigateColor, alertedColor, attention.AttentionValue / attention.AttentionThreshold);
        }
    }
}
