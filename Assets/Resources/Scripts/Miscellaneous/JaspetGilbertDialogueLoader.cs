using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class JaspetGilbertDialogueLoader : MonoBehaviour
{
    [SerializeField] private string jasperDialogue, gilbertDialogue;

    void Start()
    {
        Talker talker = GetComponent<Talker>();

        if (PlayerChoices.Instance().HasHistorianGhost)
        {
            if (gilbertDialogue.Equals(""))
                Destroy(gameObject);
            else
            {
                talker.TalkerName = "Gilbert";
                talker.DialogueName = gilbertDialogue;
                VD.LoadDialogues(talker.DialogueName, "");
            }
        }
        else
        {
            if (jasperDialogue.Equals(""))
                Destroy(gameObject);
            else
            {
                talker.TalkerName = "Jasper";
                talker.DialogueName = jasperDialogue;
                VD.LoadDialogues(talker.DialogueName, "");
            }
        }
    }
}
