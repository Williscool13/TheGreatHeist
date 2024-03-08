using Febucci.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueResponse : MonoBehaviour
{

    [TextArea(5,15)][SerializeField] private string dialogue;
    
    [SerializeField] private TypewriterByCharacter typeWriter;


    private void Start() {
        typeWriter.ShowText("");
    }

    [Button("Test Dialogue")]
    public void WriteDialogue() {
        typeWriter.ShowText(dialogue);
    }

}
