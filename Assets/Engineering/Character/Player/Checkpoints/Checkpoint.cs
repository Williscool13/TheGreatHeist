using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCheckpoints
{
    public class Checkpoint : MonoBehaviour
    {

        [SerializeField] private int checkpointId = 0;
        public int CheckpointId => checkpointId;
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision != null && !collision.CompareTag("Player")) return;

            PlayerCheckpointManager pcm = collision.GetComponentInChildren<PlayerCheckpointManager>();
            if (pcm == null) return;

            Debug.Log("Registered Checkpoint with ID: " + checkpointId + " at position: " + transform.position.ToString()); 
            bool resp = pcm.RegisterCheckpoint(this);
            if (resp) {
                // feedback with some floating text or some particle effects
            } 
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

    

}
