using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Checkpoint Data")]
public class CheckpointData : ScriptableObject
{
    public CheckpointInformation Checkpoint { get; set; }
}

public struct CheckpointInformation
{
    public string sceneName;
    public Vector3 position;
    public Vector3 rotation;
}




