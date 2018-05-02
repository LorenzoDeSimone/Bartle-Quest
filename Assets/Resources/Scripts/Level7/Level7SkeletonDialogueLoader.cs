using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void DisableDialogue()
    {
        Destroy(gameObject);
    }
}
