using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : Interactable
{
    [HideInInspector] public static bool isPossibleToTalk = true;
    [SerializeField] private string dialogueName;
    [SerializeField] private string talkerName;
    [SerializeField] private Animator actualTalkerAnimator;
    private DialogueManager dialogueManager;

    public string TalkerName
    {
        get { return talkerName; }
        set { talkerName = value; }
    }

    public string DialogueName
    {
        get { return dialogueName; }
        set { dialogueName = value; }
    }

    public Animator ActualTalkerAnimator
    {
        get
        {
            if (actualTalkerAnimator)
                return actualTalkerAnimator;
            else
                return GetComponent<Animator>();
        }

        set { actualTalkerAnimator = value; }
    }

    // Use this for initialization
    void Start ()
    {
        dialogueManager = GameObject.Find("CanvasPlayerUI").GetComponent<DialogueManager>();
    }

    public override bool CanInteract()
    {
        return dialogueName != null;
    }

    public override void Interact()
    {
        dialogueManager.InitDialogue(this);
    }
}
