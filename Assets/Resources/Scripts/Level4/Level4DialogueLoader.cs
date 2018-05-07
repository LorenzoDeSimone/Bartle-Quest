using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4DialogueLoader : MonoBehaviour
{
    [SerializeField] private GameObject Spike;

    // Use this for initialization
    void Awake ()
    {
        if (!PlayerChoices.Instance().HelpedSpikeWithoutReward)
        {
            gameObject.SetActive(false);
        }
    }
}
