using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;

    [Title("Interact Properties")]
    [SerializeField] private float interactDistance = 0.5f;
    [SerializeField] private float interactRadius = 0.5f;
    [SerializeField] private LayerMask interactObjectLayer;


    IInteractable currentHighlight;
    IInteractable nextHighlight;
    bool attempHighlight = false;

    public void HighlightInteract() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GetInteractPoint(), interactRadius);
        float closestDistance = float.MaxValue;
        IInteractable closestInteactable = null;
        foreach (Collider2D collider in colliders) {
            if (!collider.TryGetComponent(out IInteractable interactable)) continue;

            float dist = Vector2.Distance(collider.ClosestPoint(transform.position), transform.position);
            if (dist < closestDistance) {
                closestDistance = dist;
                closestInteactable = interactable;
            }
        }

        if (closestInteactable != null) {
            nextHighlight = closestInteactable;
        }
        attempHighlight = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetInteractPoint(), interactRadius);
    }

    private void LateUpdate() {
        if (!attempHighlight && currentHighlight != null) {
            currentHighlight.Unhighlight();
            currentHighlight = null;
            return;
        }

        if (currentHighlight == nextHighlight) { return; }
        if (currentHighlight != null) { currentHighlight.Unhighlight(); }
        if (nextHighlight != null) { nextHighlight.Highlight(); }
        currentHighlight = nextHighlight;
        nextHighlight = null;

        attempHighlight = false;
    }

    public void Interact() {
        if (currentHighlight != null) {
            currentHighlight.Interact();
        }
    }


    Vector2 GetInteractPoint() {
        Vector2 crosshair = playerAim.GetMousePositionWorld();
        Vector2 playerPos = transform.position;
        Vector2 direction = crosshair - playerPos;
        if (direction.magnitude > interactDistance) {
            direction = direction.normalized * interactDistance;
        }
        return playerPos + direction;
    }
}
