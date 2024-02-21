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
            if (collision == null) return;
            PlayerCheckpointManager pcm = collision.GetComponentInChildren<PlayerCheckpointManager>();
            if (pcm == null) return;
            bool resp = pcm.RegisterCheckpoint(this);
            if (resp) {
                // feedback with some floating text or some particle effects
            } 
        }
    }

}
