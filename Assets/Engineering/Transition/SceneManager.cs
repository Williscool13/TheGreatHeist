using ScriptableObjectDependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameSceneManager
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; private set; }
        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        string targetScene;
        [SerializeField] private NullEvent sceneChangeStart;
        public void LoadScene(string target) {
            TransitionManager.Instance.Transition(0.5f, 0.75f, null);
            TransitionManager.Instance.transitionLinger += OnTransitionLinger_ChangeScene;
            targetScene = target;
        }

        void OnTransitionLinger_ChangeScene(object o, EventArgs e) {
            sceneChangeStart.Raise(null);
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
            TransitionManager.Instance.transitionLinger -= OnTransitionLinger_ChangeScene;
        }
    }

    public static class SceneReferences {
        public static string Sandbox = "SandboxScene";
        public static string Overworld = "Overworld";
        public static string Underground = "Underground";
        public static string Final = "Final";
        public static string Credits = "Credits";
    }
}
