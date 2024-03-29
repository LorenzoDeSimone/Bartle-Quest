﻿using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class PuzzleGhost : MonoBehaviour
{
    private static Dictionary<string, PuzzleGhost> nameToGhostPosition;
    [SerializeField] private Transform currentGhost;
    [SerializeField] private Transform correctGhost;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private string dialogueName;
    [SerializeField] private Target talkingHeads;
    [SerializeField] private Transform ghostHead;

    private Talker talker;

    public Transform CurrentGhost
    {
        get { return currentGhost; }
        set { currentGhost = value; }
    }

    void Start()
    {
        if (nameToGhostPosition == null)
            nameToGhostPosition = new Dictionary<string, PuzzleGhost>();

        talker = GetComponent<Talker>();
        nameToGhostPosition[currentGhost.name] = this;
        talker.DialogueName = currentGhost.name;
        VD.LoadDialogues(talker.DialogueName, "");
    }

    void Update()
    {

        /*foreach (string s in nameToGhostPosition.Keys)
        {
            Debug.Log(s);
        }*/
    }

    public void SwapWith(string newGhostName)
    {
        //This is terrible. I know. This is horrible.

        //GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
        //spawnEffectGO.transform.position = targetPosition;
        PuzzleGhost oldGhostPosition = dialogueManager.GetCurrentTalker().GetComponent<PuzzleGhost>();
        PuzzleGhost newGhostPosition;

        //foreach (string s in nameToGhostPosition.Keys)
        //    Debug.Log(s);

        if (nameToGhostPosition.TryGetValue(newGhostName, out newGhostPosition))
        {
            nameToGhostPosition[newGhostName] = oldGhostPosition;
            nameToGhostPosition[oldGhostPosition.CurrentGhost.name] = newGhostPosition;

            newGhostPosition.talker.DialogueName = oldGhostPosition.talker.DialogueName;
            oldGhostPosition.talker.DialogueName = newGhostName;

            Vector3 swapPosition = oldGhostPosition.CurrentGhost.position;
            oldGhostPosition.CurrentGhost.position = newGhostPosition.currentGhost.position;
            newGhostPosition.CurrentGhost.position = swapPosition;

            Transform swapGhost = oldGhostPosition.currentGhost;
            oldGhostPosition.CurrentGhost = newGhostPosition.CurrentGhost;
            newGhostPosition.CurrentGhost = swapGhost;

            SpawnEffect(oldGhostPosition.CurrentGhost.position, oldGhostPosition.CurrentGhost.name);
            SpawnEffect(newGhostPosition.CurrentGhost.position, newGhostPosition.CurrentGhost.name);

            if (CheckRightCombination())
            {
                BartleStatistics.Instance().IncrementSocializer();
                DisableAllInteractions();
                talkingHeads.gameObject.SetActive(true);
                AudioManager.Instance().PlaySuccess();
            }
        }

        /*foreach (string s in nameToGhostPosition.Keys)
        {
            Debug.Log(s + " "+ nameToGhostPosition[s].name);
        }*/
    }

    private void SpawnEffect(Vector3 position, string s)
    {
        string attach;

        if (s.Equals("GhostRed"))
            attach = "Red";
        else if (s.Equals("GhostGreen"))
            attach = "Green";
        else if (s.Equals("GhostYellow"))
            attach = "Yellow";
        else
            attach = "Blue";

        UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffect"+attach);
        GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
        spawnEffectGO.transform.position = position;
    }

    private void DisableAllInteractions()
    {
        foreach (string s in nameToGhostPosition.Keys)
        {
            SpawnEffect(nameToGhostPosition[s].CurrentGhost.position, nameToGhostPosition[s].CurrentGhost.name);
            Destroy(nameToGhostPosition[s].gameObject);
            Destroy(nameToGhostPosition[s].CurrentGhost.gameObject);
        }
    }

    private bool CheckRightCombination()
    {
        PuzzleGhost ghostPosition;
        foreach(string s in nameToGhostPosition.Keys)
        {
            ghostPosition = nameToGhostPosition[s];
            //Debug.Log(ghostPosition.currentGhost.name + "  " + ghostPosition.correctGhost.name);
            if (!ghostPosition.CurrentGhost.Equals(ghostPosition.correctGhost))
                return false;
        }

        return true;
    }
}
