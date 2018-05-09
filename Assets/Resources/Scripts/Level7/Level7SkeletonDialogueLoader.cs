using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class Level7SkeletonDialogueLoader : MonoBehaviour
{
    [SerializeField] string canTalkWithSkeletonsDialogue, cannotTalkWithSkeletonsDialogue;
	// Use this for initialization
	void Start ()
    {
        if (PlayerChoices.Instance().CanSpeakWithSkeletons)
            GetComponent<Talker>().DialogueName = canTalkWithSkeletonsDialogue;
        else
            GetComponent<Talker>().DialogueName = cannotTalkWithSkeletonsDialogue;

        VD.LoadDialogues(GetComponent<Talker>().DialogueName, "");
    }

    public void DisableDialogue()
    {
        Destroy(gameObject);
    }
}
