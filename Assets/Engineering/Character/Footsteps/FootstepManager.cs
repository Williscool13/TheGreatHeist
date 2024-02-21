using FootstepSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] private FootstepSwapper swapper;
    [SerializeField] private AudioSource footstepSource;

    FootstepCollection currentCollection;
    bool isStairs;

    public bool IsStairs {
        get {
            return isStairs;
        }
    }

    public void PlayWalk() {
        GetCollection();
        if (currentCollection == null) Debug.Log("Current collection is null.");
        footstepSource.PlayOneShot(currentCollection.GetRandomWalkClip());
    }

    public void PlayRun() {
        GetCollection();
        if (currentCollection == null) Debug.Log("Current collection is null.");
        footstepSource.PlayOneShot(currentCollection.GetRandomRunSound());
    }

    public void PlayJump() {
        GetCollection();
        if (currentCollection == null) Debug.Log("Current collection is null.");
        footstepSource.PlayOneShot(currentCollection.GetRandomJumpSound());
    }

    public void PlayLand() {
        GetCollection();
        if (currentCollection == null) Debug.Log("Current collection is null.");
        footstepSource.PlayOneShot(currentCollection.GetRandomLandSound());
    }

    void GetCollection() {
        SurfaceInfo si = swapper.SwapFootsteps();
        if (si.hasSurface) {
            currentCollection = si.collection;
            isStairs = si.stairs;
        } else {
            Debug.Log("No surface found for footstep point.");
        }
    }

    [Button("Test Walk")]
    void TestWalk() {
        PlayWalk();
    }

    [Button("Test Land")]
    void TestLand() {
        PlayLand();
    }
}
