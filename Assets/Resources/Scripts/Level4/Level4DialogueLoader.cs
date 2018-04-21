using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4DialogueLoader : MonoBehaviour
{
    [SerializeField] private GameObject Spike;
    [SerializeField] private string SpikeDialogueName, AloneDialogueName;

    // Use this for initialization
    void Awake ()
    {
	    if(PlayerChoices.Instance().HelpedSpikeWithoutReward)
        {
            Spike.SetActive(true);
            GetComponent<Talker>().DialogueName = SpikeDialogueName;
        }
        else
            GetComponent<Talker>().DialogueName = AloneDialogueName;
    }
}
