using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerCheckpoints
{
    public class PlayerCheckpointManager : MonoBehaviour
    {
        [SerializeField][ReadOnly] int highestCheckpointId = -1;
        [SerializeField][ReadOnly] Checkpoint currentCheckpoint = null;

        [SerializeField] private Checkpoint initialCheckpoint;
        private void Start() {
            Debug.Assert(initialCheckpoint != null, "Must set an initial checkpoint");
            highestCheckpointId = initialCheckpoint.CheckpointId;
            currentCheckpoint = initialCheckpoint;
        }

        public bool RegisterCheckpoint(Checkpoint point) {
            if (point.CheckpointId < highestCheckpointId) return false;
            highestCheckpointId = point.CheckpointId;
            currentCheckpoint = point;
            return true;
        }

        public Checkpoint GetCheckpoint() {
            return currentCheckpoint;
        }



    }
}
