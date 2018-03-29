using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoices : MonoBehaviour
{
    [SerializeField] private bool helpedSpikeWithoutReward = false;

    private static PlayerChoices instance;
    private static PlayerChoices rollbackInstance; 

    void Awake()
    {
        Instance();
        rollbackInstance = (PlayerChoices)instance.MemberwiseClone();
    }

    public static PlayerChoices Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<PlayerChoices>();

        return instance;
    }

    public void Rollback()
    {
        instance = rollbackInstance;
    }

    public bool HelpedSpikeWithoutReward
    {
        set { Instance().helpedSpikeWithoutReward = value; }
        get { return Instance().helpedSpikeWithoutReward;  }
    }
}
