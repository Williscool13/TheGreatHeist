using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttentionMark : MonoBehaviour
{
    [Title("Follow Target")]
    [SerializeField] Transform followTarget;
    [SerializeField] float heightOffset = 1.3f;

    [Title("Color")]
    [SerializeField] EnemyAttention attention;
    [SerializeField] Color alertedColor = Color.red;
    [SerializeField] Color investigateColor = new Color(255.0f / 255.0f, 165.0f / 255.0f, 0.0f);
    [SerializeField] Color idleColor = Color.yellow;

    [SerializeField] SpriteRenderer spriteRenderer;

    private void Start() {
        transform.parent = null;
    }

    void Update() {
        transform.position = followTarget.position + new Vector3(0, heightOffset, 0);
        transform.rotation = Quaternion.identity;

        if (attention.AttentionValue <= 0) {
            spriteRenderer.enabled = false;
        }
        else if (attention.AttentionValue >= attention.AttentionThreshold) {
            spriteRenderer.enabled = true;
            spriteRenderer.color = alertedColor;
        }
        else {
            spriteRenderer.enabled = true;
            spriteRenderer.color = Color.Lerp(idleColor, investigateColor, attention.AttentionValue / attention.AttentionThreshold);
        }
    }
}
