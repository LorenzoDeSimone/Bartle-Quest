using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject interactionPopup;
    [SerializeField] private Text nameField;
    [SerializeField] private Text dialogueField;

    //Inspector hierarchy: Panel(Image) -> ChoiceText (Text) -> HighlightBorder(Image)
    [SerializeField] private GameObject[] choiceFields;

    private Talker currentTalker;

    private bool canGoOn;
    private bool ended;

    private static bool isDialogueOn;

    private int currentChoiceIndex = 0;

    public static bool IsDialogueOn
    {
        get { return isDialogueOn;}
    }

    public bool CanGoOn()
    {
        return canGoOn;
    }

    public void ChangeDisplayedTalkerName(string name)
    {
        nameField.text = name;
    }

    public void InitDialogue(Talker talker)
    {
        if (!Talker.isPossibleToTalk || isDialogueOn)
            return;

        isDialogueOn = true;
        canGoOn = true;
        currentChoiceIndex = 0;
        ToggleChoice(choiceFields[currentChoiceIndex], true);

        currentTalker = talker;
        string dialogue = talker.DialogueName;
        nameField.text = talker.TalkerName;

        gameObject.AddComponent<VD>();
        VIDE_Assign videDialogue = GetComponent<VIDE_Assign>();
        videDialogue.AssignNew(dialogue);
        VD.BeginDialogue(videDialogue);
        //TO DO:
        //Go to the correct dialogue part thanks to info in talker
        Time.timeScale = 0f;
        dialoguePanel.SetActive(true);
        interactionPopup.SetActive(false);

        if (currentTalker.GetComponent<Animator>())
            currentTalker.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;

        StartCoroutine(DialogueRoutine());
    }

    private void PlayerNode(VD.NodeData nodeData)
    {
        for (int i = 0; i < choiceFields.Length; i++)
        {
            if (i < nodeData.comments.Length)
            {
                choiceFields[i].SetActive(true);
                choiceFields[i].GetComponentInChildren<Text>().text = nodeData.comments[i];
            }
            else
                choiceFields[i].SetActive(false);
        }
        canGoOn = false;
        //VD.Next();
    }

    private void NPCNode(VD.NodeData nodeData)
    {
        string fullText = nodeData.comments[nodeData.commentIndex];
        dialogueField.text = fullText;
        VD.Next();
    }

    public void HightLightChoice(int direction)
    {
        ToggleChoice(choiceFields[currentChoiceIndex], false);

        currentChoiceIndex += direction;
        if (currentChoiceIndex == VD.nodeData.comments.Length)
            currentChoiceIndex = 0;
        else if (currentChoiceIndex < 0)
            currentChoiceIndex = VD.nodeData.comments.Length - 1;

        ToggleChoice(choiceFields[currentChoiceIndex], true);
        //Debug.Log(currentChoiceIndex);
    }

    private void ToggleChoice(GameObject choicePanel, bool value)
    {
        //Debug.Log(choiceText.GetComponentInChildren<Image>());
        if (choicePanel.GetComponentInChildren<Text>())
        {
            Image borderHighlight = choicePanel.GetComponentInChildren<Text>().GetComponentInChildren<Image>();
            Color oldColor = borderHighlight.GetComponentInChildren<Image>().color;
            float alpha;
            if (value)
                alpha = 220;
            else
                alpha = 0;

            borderHighlight.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        }
    }

    public void DialogueChoiceConfirmed()
    {
        VD.nodeData.commentIndex = currentChoiceIndex;
        VD.Next();
        ToggleChoice(choiceFields[currentChoiceIndex], false);
        currentChoiceIndex = 0;
        ToggleChoice(choiceFields[0], true);
        canGoOn = true;
    }

    IEnumerator DialogueRoutine()
    {
        while (VD.isActive && !ended)
        {
            if (VD.nodeData.isPlayer)// If it's a player node, let's show all of the available options as buttons
                PlayerNode(VD.nodeData);
            else
                NPCNode(VD.nodeData);

            if (VD.nodeData.isEnd) // If it's the end, let's just call EndDialogue
                EndDialogue();

            if(!canGoOn)
                yield return new WaitUntil(CanGoOn);
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        interactionPopup.SetActive(true);
        VD.EndDialogue();
        Time.timeScale = 1f;
        isDialogueOn = false;

        if (currentTalker.GetComponent<Animator>())
            currentTalker.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
    }
}
