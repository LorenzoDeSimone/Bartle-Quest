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
    private float hardcap = 0.5f;

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
    
    
    void Update()
    {
        Debug.Log("|| A: "+ Instance().ACHIEVER + " || " + "E: " + Instance().EXPLORER + "|| S: " + Instance().SOCIALIZER + " || K: " + Instance().KILLER + " || "+ Instance().totalChoices);
    }
    

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
        
        /*
        results[ARCHETYPE.ACHIEVER] = 0.56f;//Instance().ACHIEVER / totalChoices;
        results[ARCHETYPE.EXPLORER] = 0.125f;//Instance().EXPLORER / totalChoices;
        results[ARCHETYPE.SOCIALIZER] = 0.0625f;//Instance().SOCIALIZER / totalChoices;
        results[ARCHETYPE.KILLER] = 0.25f;// Instance().KILLER / totalChoices;
        */

        foreach (ARCHETYPE archetype in ARCHETYPE.GetValues(typeof(ARCHETYPE)))
        {
            if (results[archetype] > hardcap)
            {
                //If a value exceeds the hardcap, the delta is distributed evenly to all other archetypes
                float delta = (results[archetype] - hardcap) / (results.Count - 1);
                results[archetype] = hardcap;

                if (archetype.Equals(ARCHETYPE.ACHIEVER))
                {
                    results[ARCHETYPE.EXPLORER] += delta;
                    results[ARCHETYPE.SOCIALIZER] += delta;
                    results[ARCHETYPE.KILLER] += delta;
                }
                else if (archetype.Equals(ARCHETYPE.EXPLORER))
                {
                    results[ARCHETYPE.SOCIALIZER] += delta;
                    results[ARCHETYPE.ACHIEVER] += delta;
                    results[ARCHETYPE.KILLER] += delta;
                }
                else if (archetype.Equals(ARCHETYPE.SOCIALIZER))
                {
                    results[ARCHETYPE.EXPLORER] += delta;
                    results[ARCHETYPE.ACHIEVER] += delta;
                    results[ARCHETYPE.KILLER] += delta;
                }
                else if (archetype.Equals(ARCHETYPE.KILLER))
                {
                    results[ARCHETYPE.EXPLORER] += delta;
                    results[ARCHETYPE.SOCIALIZER] += delta;
                    results[ARCHETYPE.ACHIEVER] += delta;
                }
            }
        }
        return results;
    }
}
