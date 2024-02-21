using DG.Tweening;
using PlayerCheckpoints;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    [Title("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private PlayerCheckpointManager checkpointManager;

    [Title("Time")]
    [SerializeField] private float respawnInitialDelay = 1.0f;
    [SerializeField] private float respawnTransitionLingertime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem.OnDeath += OnDeath;
    }

    private void OnDeath(object sender, System.EventArgs e) {
        // slow down time and have the death animation play. have a manager for this.
        // 1/2 speed gore-y blood particle system
        // circle wipe out after a hard coded delay
        TransitionManager.Instance.Transition(respawnInitialDelay, respawnTransitionLingertime, playerTransform);
        // after a hard coded delay, respawn the player at tne previous checkpoint.
        TransitionManager.Instance.transitionLinger += OnTransitionLinger;
        TransitionManager.Instance.transitionEnd += OnTransitionEnd;
        Debug.Log("Player has died");
        // after respawning has finished, refill the player's health bar, probably not to full. And re-enable the player's state machine.
    }

    void OnTransitionLinger(object sender, System.EventArgs e) {
        Checkpoint c = checkpointManager.GetCheckpoint();
        playerTransform.position = c.transform.position;
        playerTransform.rotation = c.transform.rotation;

        (sender as TransitionManager).transitionLinger -= OnTransitionLinger;
    }

    void OnTransitionEnd(object sender, System.EventArgs e) {
        healthSystem.SetHealth(Mathf.Max(healthSystem.HealthData.maxHealth / 2, 1));

        (sender as TransitionManager).transitionEnd -= OnTransitionEnd;
    }
}
