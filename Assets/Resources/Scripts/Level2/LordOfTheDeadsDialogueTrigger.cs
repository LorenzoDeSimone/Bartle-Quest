using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class LordOfTheDeadsDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] Transform barrier;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
        {
            if (PlayerChoices.Instance().BlessedSword)
            {
                GetComponent<Talker>().DialogueName = "LordOfTheDeadsExplorer";
                VD.LoadDialogues("LordOfTheDeadsExplorer", "");

            }
            else
            {
                GetComponent<Talker>().DialogueName = "LordOfTheDeadsAchiever";
                VD.LoadDialogues("LordOfTheDeadsAchiever", "");
            }

            dialogueManager.InitDialogue(GetComponent<Talker>());
            barrier.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
