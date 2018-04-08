using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyGhostDialogue : MonoBehaviour
{
    [SerializeField] public DialogueManager dialogueManager;

    public void SpawnDialogue(string dialogueName)
    {
        StartCoroutine(PopDialogue(dialogueName));
    }

    IEnumerator PopDialogue(string dialogueName)
    {
        Talker talker = GetComponent<Talker>();
        talker.DialogueName = dialogueName;
        yield return new WaitForSeconds(1.5f);
        dialogueManager.InitDialogue(talker);
    }
}
