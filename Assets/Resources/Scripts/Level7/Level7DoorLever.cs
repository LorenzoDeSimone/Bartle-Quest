using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7DoorLever : Interactable
{
    [SerializeField] private Transform prisoners;
    [SerializeField] private Transform prisonersFinalPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string dialogueName;

    [SerializeField] private GameObject[] Enemies;

    private Animation animation;

    private bool leverPulled = false;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public override void Interact()
    {
        animation.Play();
        leverPulled = true;
        GetComponent<Target>().enabled = false;

        prisoners.position = prisonersFinalPosition.position;
        //prisoners.rotation = Quaternion.LookRotation(player.transform.position - prisoners.position, Vector3.up);
    }

    public override bool CanInteract()
    {
        if (leverPulled)
        {
            return false;
        }

        foreach (GameObject go in Enemies)
        {
            if (go != null)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator StartDialogueWithNPC(Talker npcTalker)
    {
        yield return new WaitForSeconds(0.5f);
        dialogueManager.InitDialogue(npcTalker);
    }


}

