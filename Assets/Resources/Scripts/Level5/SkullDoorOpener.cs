using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoorOpener : Interactable
{
    [SerializeField] private Transform skullLight;
    [SerializeField] private ExplodingDoor[] doors;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] string normalLeverDialogue, finalLeverDialogue;

    private static int nLevers = 0;
    private static int nLeversPulled;

    private bool leverPulled = false;
    private Animation animation; 

    void Awake()
    {
        nLevers++;
        animation = GetComponent<Animation>();
    }

    public override bool CanInteract()
    {
        return !leverPulled;
    }

    public override void Interact()
    {
        leverPulled = true;
        animation.Play();
        skullLight.gameObject.SetActive(true);

        nLeversPulled++;
        Talker talker = GetComponent<Talker>();

        if (nLeversPulled < nLevers)
        {
            AudioManager.Instance().PlaySuccess();
            talker.DialogueName = normalLeverDialogue;
            dialogueManager.InitDialogue(talker);
        }
        else if (nLeversPulled >= nLevers)
        {
            foreach (ExplodingDoor door in doors)
            {
                if (door)
                   door.Explode();

                talker.DialogueName = finalLeverDialogue;
                dialogueManager.InitDialogue(talker);
            }
        }
    }

    void OnDestroy()
    {
        nLevers = 0;
        nLeversPulled = 0;
    }
}