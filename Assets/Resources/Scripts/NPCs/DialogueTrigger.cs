using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Talker myTalker;
    [SerializeField] private bool oneShot = true;
    private bool dialogueFired = false;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>() && myTalker && !dialogueFired)
        {
            dialogueManager.InitDialogue(myTalker);
            if (oneShot)
                dialogueFired = true;
        }
    }
}
