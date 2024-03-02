using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Data/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private string dialogue;

    public string Dialogue { get => dialogue; }
}
