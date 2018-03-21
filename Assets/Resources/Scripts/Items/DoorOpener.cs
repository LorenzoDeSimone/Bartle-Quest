﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : Interactable
{
    [SerializeField] private ExplodingDoor[] doorsToOpen;
    [SerializeField] private Transform NPC;
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string dialogueName;

    [SerializeField] private GameObject[] Enemies;

    public override void Interact()
    {
        if (!CanInteract())
            return;

        foreach (ExplodingDoor d in doorsToOpen)
        {
            if(d)
                d.Explode();

            NPC.position = new Vector3 (doorsToOpen[0].transform.position.x, NPC.position.y, doorsToOpen[0].transform.position.z);
            NPC.rotation = Quaternion.LookRotation(player.transform.position - NPC.position, Vector3.up);

            Talker npcTalker = NPC.GetComponent<Talker>();
            NPC.GetComponent<Talker>().enabled = false;
            NPC.GetComponent<Target>().enabled = false;
            npcTalker.DialogueName = dialogueName;
            StartCoroutine(StartDialogueWithNPC(npcTalker));
        }
    }

    public new bool CanInteract()
    {
        foreach(GameObject go in Enemies)
        {
            if (go != null)
                return false;
        }
        return true;
    }

    private IEnumerator StartDialogueWithNPC(Talker npcTalker)
    {
        yield return new WaitForSeconds(1f);
        dialogueManager.InitDialogue(npcTalker);
    }

    
}
