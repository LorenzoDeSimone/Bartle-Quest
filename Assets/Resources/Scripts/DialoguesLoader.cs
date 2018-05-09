using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class DialoguesLoader : MonoBehaviour
{
	void Start ()
    {
        VD.UnloadDialogues();

        var allTalkersInScene = FindObjectsOfType<Talker>();
        foreach (Talker t in allTalkersInScene)
        {
            if (t.DialogueName.Equals(null) || t.DialogueName.Equals(""))
                continue;

            VD.LoadDialogues(t.DialogueName, "");
        }
	}
	
}
