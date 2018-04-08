using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] Transform barrier;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
        {
            if (PlayerChoices.Instance().BlessedSword)
                GetComponent<Talker>().DialogueName = "LordOfTheDeadsExplorer";
            else
                GetComponent<Talker>().DialogueName = "LordOfTheDeadsAchiever";

            dialogueManager.InitDialogue(GetComponent<Talker>());
            barrier.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
