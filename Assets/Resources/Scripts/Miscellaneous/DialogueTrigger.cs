using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] Talker talker;
    [SerializeField] bool oneShot = true;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
        {
            dialogueManager.InitDialogue(talker);
            if (oneShot)
                Destroy(gameObject);
        }
    }
}
