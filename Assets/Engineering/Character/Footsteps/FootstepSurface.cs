using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootstepSystem
{
    public class FootstepSurface : MonoBehaviour
    {
        [SerializeField] private FootstepCollection footstepCollection;
        [SerializeField] private bool isStairs;
        public FootstepCollection Footsteps {
            get {
                return footstepCollection;
            }
        }

        public bool IsStairs {
            get {
                return isStairs;
            }
        }
    }
}
