using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5DoorDialogueLoader : MonoBehaviour {

    [SerializeField] string hatDialogue, noHatDialogue;

	// Use this for initialization
	void Start ()
    {
        if (PlayerChoices.Instance().HasWeirdHat)
            GetComponent<Talker>().DialogueName = hatDialogue;
        else
            GetComponent<Talker>().DialogueName = noHatDialogue;
    }

}
