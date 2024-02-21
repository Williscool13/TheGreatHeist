using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootstepSystem
{
    public class FootstepSwapper : MonoBehaviour
    {
        [SerializeField] private Transform footstepPoint;
        [SerializeField] private float footstepRadius = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        public SurfaceInfo SwapFootsteps() {
            Collider2D col = Physics2D.OverlapCircle(footstepPoint.position, footstepRadius, groundLayer);
            //Collider2D col = Physics2D.OverlapPoint(footstepPoint.position, groundLayer);
            if (col == null) { Debug.Log("No surface found for the footstep point"); return new SurfaceInfo { hasSurface = false }; }
            FootstepSurface surface = col.GetComponent<FootstepSurface>();
            if (surface == null) { Debug.Log("No footstep collection found for the surface"); return new SurfaceInfo { hasSurface = false }; }
            return new SurfaceInfo { hasSurface = true, collection = surface.Footsteps, stairs = surface.IsStairs };
        }
    }

    public struct SurfaceInfo {
        public bool hasSurface;
        public FootstepCollection collection;
        public bool stairs;
    }
}
