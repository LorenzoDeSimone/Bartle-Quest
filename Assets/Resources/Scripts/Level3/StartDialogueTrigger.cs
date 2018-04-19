using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
        {
            dialogueManager.InitDialogue(GetComponent<Talker>());
            Destroy(gameObject);
        }
    }
}
