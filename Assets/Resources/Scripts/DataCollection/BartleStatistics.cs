using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BartleStatistics : MonoBehaviour
{
    private static BartleStatistics instance;

    public enum ARCHETYPE { ACHIEVER, EXPLORER, SOCIALIZER, KILLER}

    float ACHIEVER = 0f, EXPLORER = 0f, SOCIALIZER = 0f, KILLER = 0f;

    public static BartleStatistics Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<BartleStatistics>();

        return instance;
    }
    
    /*
    void Update()
    {
        Debug.Log("|| A: "+ ACHIEVER + " || " + "E: " + EXPLORER + "|| S: " + SOCIALIZER + " || K: " + KILLER + " || ");
    }*/

    public void IncrementAchiever()   { Instance().ACHIEVER++;   }

    public void IncrementExplorer()   { Instance().EXPLORER++;   }

    public void IncrementSocializer() { Instance().SOCIALIZER++; }

    public void IncrementKiller()     { Instance().KILLER++;     }

    public void Reset()
    {
        Instance().ACHIEVER = Instance().EXPLORER = Instance().SOCIALIZER = Instance().KILLER = 0f;
    }

    public Dictionary<ARCHETYPE, float> GetResults(int totalQuestions)
    {
        Dictionary<ARCHETYPE, float> results = new Dictionary<ARCHETYPE, float>();

        results[ARCHETYPE.ACHIEVER]   = Instance().ACHIEVER   / totalQuestions;
        results[ARCHETYPE.EXPLORER]   = Instance().EXPLORER   / totalQuestions;
        results[ARCHETYPE.SOCIALIZER] = Instance().SOCIALIZER / totalQuestions;
        results[ARCHETYPE.KILLER]     = Instance().KILLER     / totalQuestions;

        return results;
    }
}
