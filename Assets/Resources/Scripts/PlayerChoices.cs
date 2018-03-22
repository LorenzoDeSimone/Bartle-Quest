using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoices : MonoBehaviour
{
    [SerializeField] private bool helpedSpikeWithoutReward = false;

    private static PlayerChoices instance;

    public static PlayerChoices Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<PlayerChoices>();

        return instance;
    }

    public bool HelpedSpikeWithoutReward
    {
        set { Instance().helpedSpikeWithoutReward = value; }
        get { return Instance().helpedSpikeWithoutReward;  }
    }
}
