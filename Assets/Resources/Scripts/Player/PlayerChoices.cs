using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoices : MonoBehaviour
{
    [SerializeField] private Dictionary<string, float> levelRatings;

    [SerializeField] private bool isMale = true;
    [SerializeField] private bool helpedSpikeWithoutReward = false;
    [SerializeField] private bool blessedSowrd = false;
    [SerializeField] private bool canSeeEnemyHP = false;
    [SerializeField] private bool hasWeirdHat = false;
    [SerializeField] private bool canUseExplosionSpell = false;
    [SerializeField] private bool canSeeHiddenWalls = false;
    [SerializeField] private bool canSpeakWithSkeletons = false;
    [SerializeField] private bool hasHistorianGhost = false;
    [SerializeField] private bool lv7AlarmTriggered = false;
    [SerializeField] private bool lv7SkeletonControlled = false;

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

    public bool CanSeeEnemyHP
    {
        set { Instance().canSeeEnemyHP = value; }
        get { return Instance().canSeeEnemyHP; }
    }

    public bool HasWeirdHat
    {
        set { Instance().hasWeirdHat = value; }
        get { return Instance().hasWeirdHat; }
    }

    public bool CanUseExplosionSpell
    {
        set { Instance().canUseExplosionSpell = value; }
        get { return Instance().canUseExplosionSpell; }
    }
  

    public bool IsMale
    {
        set { Instance().isMale = value; }
        get { return Instance().isMale; }
    }


    public bool CanSeeHiddenWalls
    {
        set { Instance().canSeeHiddenWalls = value; }
        get { return Instance().canSeeHiddenWalls; }
    }

    public bool CanSpeakWithSkeletons
    {
        set { Instance().canSpeakWithSkeletons = value; }
        get { return Instance().canSpeakWithSkeletons; }
    }

    public bool HasHistorianGhost
    {
        set { Instance().hasHistorianGhost = value; }
        get { return Instance().hasHistorianGhost; }
    }

    public bool Lv7AlarmTriggered
    {
        set { Instance().lv7AlarmTriggered = value; }
        get { return Instance().lv7AlarmTriggered; }
    }

    public bool Lv7SkeletonControlled
    {
        set { Instance().lv7SkeletonControlled = value; }
        get { return Instance().lv7SkeletonControlled; }
    }

    public void AddLevelRating(string levelName, float levelRating)
    {
        Instance().levelRatings.Add(levelName,levelRating);
    }
}
