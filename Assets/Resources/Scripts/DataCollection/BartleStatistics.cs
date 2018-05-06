using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartleStatistics : MonoBehaviour
{
    private static BartleStatistics instance;
    private static BartleStatistics rollbackInstance;

    public enum ARCHETYPE { ACHIEVER, EXPLORER, SOCIALIZER, KILLER}

    float ACHIEVER = 0f, EXPLORER = 0f, SOCIALIZER = 0f, KILLER = 0f;
    private int totalChoices;

    void Awake()
    {
        Instance();
        rollbackInstance = (BartleStatistics)instance.MemberwiseClone();
    }

    public static BartleStatistics Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<BartleStatistics>();

        return instance;
    }
    
    
    /*void Update()
    {
        Debug.Log("|| A: "+ Instance().ACHIEVER + " || " + "E: " + Instance().EXPLORER + "|| S: " + Instance().SOCIALIZER + " || K: " + Instance().KILLER + " || "+ Instance().totalChoices);
    }*/
    

    public void IncrementAchiever()   { Instance().ACHIEVER++;   Instance().totalChoices++; }

    public void IncrementExplorer()   { Instance().EXPLORER++;   Instance().totalChoices++; }

    public void IncrementSocializer() { Instance().SOCIALIZER++; Instance().totalChoices++; }

    public void IncrementKiller()     { Instance().KILLER++;     Instance().totalChoices++; }

    public void Reset()
    {
        Instance().ACHIEVER = Instance().EXPLORER = Instance().SOCIALIZER = Instance().KILLER = 0f;
        totalChoices = 0;
    }

    public void Rollback()
    {
        instance = rollbackInstance;
    }

    public Dictionary<ARCHETYPE, float> GetResults()
    {
        Dictionary<ARCHETYPE, float> results = new Dictionary<ARCHETYPE, float>();

        results[ARCHETYPE.ACHIEVER]   = Instance().ACHIEVER   / totalChoices;
        results[ARCHETYPE.EXPLORER]   = Instance().EXPLORER   / totalChoices;
        results[ARCHETYPE.SOCIALIZER] = Instance().SOCIALIZER / totalChoices;
        results[ARCHETYPE.KILLER]     = Instance().KILLER     / totalChoices;

        return results;
    }
}
