using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoices : MonoBehaviour
{
    [SerializeField] private Dictionary<string, float> levelRatings;

    [SerializeField] private bool helpedSpikeWithoutReward = false;
    [SerializeField] private bool blessedSowrd = false;

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
        {
            instance = FindObjectOfType<PlayerChoices>();
            instance.levelRatings = new Dictionary<string, float>();
        }
        return instance;
    }

    /*
    void Update()
    {
        foreach (string s in levelRatings.Keys)
        {
            float v;
            levelRatings.TryGetValue(s,out v);
            Debug.Log(s + "| |" + v);
        }
    }*/

    public void Rollback()
    {
        instance = rollbackInstance;
    }

    public bool HelpedSpikeWithoutReward
    {
        set { Instance().helpedSpikeWithoutReward = value; }
        get { return Instance().helpedSpikeWithoutReward;  }
    }

    public bool BlessedSword
    {
        set { Instance().blessedSowrd = value; }
        get { return Instance().blessedSowrd; }
    }

    public void AddLevelRating(string levelName, float levelRating)
    {
        Instance().levelRatings.Add(levelName,levelRating);
    }
}
