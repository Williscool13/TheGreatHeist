using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FootstepSystem
{
    [CreateAssetMenu(fileName = "FootstepCollection", menuName = "Footsteps/Footstep Collection")]
    public class FootstepCollection : ScriptableObject
    {
        [SerializeField] AudioClip[] walkSounds;
        [SerializeField] AudioClip[] runSounds;
        [SerializeField] AudioClip[] jumpSounds;
        [SerializeField] AudioClip[] landSounds;

        public AudioClip GetRandomWalkClip() {
            return walkSounds[Random.Range(0, walkSounds.Length)];
        }
        public AudioClip GetRandomRunSound() {
            return runSounds[Random.Range(0, runSounds.Length)];
        }
        public AudioClip GetRandomJumpSound() {
            return jumpSounds[Random.Range(0, jumpSounds.Length)];
        }
        public AudioClip GetRandomLandSound() {
            return landSounds[Random.Range(0, landSounds.Length)];
        }
        public AudioClip[] GetAllWalkClips() {
            return walkSounds;
        }
        public AudioClip[] GetAllRunClips() {
            return runSounds;
        }
        public AudioClip[] GetAllJumpClips() {
            return jumpSounds;
        }
        public AudioClip[] GetAllLandClips() {
            return landSounds;
        }
    }
}
