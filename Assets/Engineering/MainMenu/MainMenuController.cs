using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using ScriptableObjectDependencyInjection;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private StringVariable inputType;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string mainVolumeParameter;

    private void Start() {
        inputType.Value = "Keyboard&Mouse";
        float volume = Mathf.Lerp(-30f, 20f, 0.6f);
        audioMixer.SetFloat(mainVolumeParameter, volume);
    }

    public void OnGamepadToggleChange(bool v) {
        if (v) {
            inputType.Value = "Gamepad";
        } else {
            inputType.Value = "Keyboard&Mouse";
        }
    }


    
    public void OnGameVolumeChange(float val) {
        float volume = Mathf.Lerp(-30f, 20f, val);
        audioMixer.SetFloat(mainVolumeParameter, volume);
    }

    public void StartGame() {
        GameSceneManager.SceneManager.Instance.LoadScene(GameSceneManager.SceneReferences.Overworld);
    }
}
