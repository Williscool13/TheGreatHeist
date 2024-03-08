using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionZone : MonoBehaviour
{
    [SerializeField] private Level nextLevel;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Player entered transition zone");
            Transition();
        }
    }

    void Transition() {
        string targetScene = nextLevel switch {
            Level.Overworld => GameSceneManager.SceneReferences.Overworld,
            Level.Underground => GameSceneManager.SceneReferences.Underground,
            Level.Final => GameSceneManager.SceneReferences.Final,
            _ => GameSceneManager.SceneReferences.Sandbox
        };
        GameSceneManager.SceneManager.Instance.LoadScene(targetScene);
    }

    [Button("To Sandbox")]
    void TestTransition() {
        Transition();
    }

    enum Level
    {
        Overworld,
        Underground,
        Final,
        None

    }
}
