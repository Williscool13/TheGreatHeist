using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FinalSceneTransitionScript : MonoBehaviour
{
    public void MoveToCredits() {
        GameSceneManager.SceneManager.Instance.LoadScene(GameSceneManager.SceneReferences.Credits);
    }

    private void Start() {
        volume.TryGet(out ChromaticAberration ca);
        if (ca != null) {
            ca.intensity.value = 0.6f;
        }
    }

    [SerializeField] private VolumeProfile volume;
    public void TurnDownAberration() {
        volume.TryGet(out ChromaticAberration ca);
        if (ca != null) {
            ca.intensity.value = 0.1f;
        }

    }
}
