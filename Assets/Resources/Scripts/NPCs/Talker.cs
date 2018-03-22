using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talker : Interactable
{
    [HideInInspector] public static bool isPossibleToTalk = true;
    [SerializeField] private string dialogueName;
    [SerializeField] private string talkerName;

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

    // Use this for initialization
    void Start ()
    {

    }

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        GameObject Canvas = GameObject.Find("CanvasPlayerUI");
        Canvas.GetComponent<DialogueManager>().InitDialogue(this);
    }
}
