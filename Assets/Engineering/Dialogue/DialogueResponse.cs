using Febucci.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResponse : MonoBehaviour
{

    [SerializeField] private string dialogue;

    [SerializeField] private TypewriterByCharacter typeWriter;

    [Button("Test Dialogue")]
    public void WriteDialogue() {
        typeWriter.ShowText(dialogue);
    }

}
