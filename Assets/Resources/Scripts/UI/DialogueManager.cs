using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Text nameField;
    [SerializeField] private Text dialogueField;
    [SerializeField] private Text[] choiceFields;

    private bool canGoOn = true;
    private bool ended;

    public bool CanGoOn()
    {
        return canGoOn;
    }

    public void InitDialogue(Talker talker)
    {
        if (!Talker.isPossibleToTalk)
            return;

        string dialogue = talker.DialogueName;
        nameField.text = talker.TalkerName;

        gameObject.AddComponent<VD>();
        VIDE_Assign videDialogue = GetComponent<VIDE_Assign>();
        videDialogue.AssignNew(dialogue);
        VD.BeginDialogue(videDialogue);
        //TO DO:
        //Go to the correct dialogue part thanks to info in talker

        StartCoroutine(DialogueRoutine());
    }

    private void PlayerNode(VD.NodeData nodeData)
    {
        for (int i = 0; i < nodeData.comments.Length; i++)
            choiceFields[i].text = nodeData.comments[i];

        canGoOn = false;
        VD.Next();
    }

    private void NPCNode(VD.NodeData nodeData)
    {
        string fullText = nodeData.comments[nodeData.commentIndex];
        dialogueField.text = fullText;
        VD.Next();
    }

    IEnumerator DialogueRoutine()
    {
        while (VD.isActive && !ended)
        {
            yield return new WaitUntil(CanGoOn);

            if (VD.nodeData.isPlayer) // If it's a player node, let's show all of the available options as buttons
            {
                PlayerNode(VD.nodeData);
            }
            else
            {
                yield return new WaitUntil(CanGoOn);
                NPCNode(VD.nodeData);
            }

            //It is here only to be sure that the panel is visible only after the text is correct
            dialoguePanel.SetActive(true);

            if (VD.nodeData.isEnd) // If it's the end, let's just call EndDialogue
                EndDialogue();
        }
    }

    private void EndDialogue()
    {
        VD.EndDialogue();
        dialoguePanel.SetActive(false);
    }
}
