using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypeTrigger : MonoBehaviour
{
    [SerializeField] string archetype;
    private bool alreadyFired = false;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>() && !alreadyFired)
        {
            if (archetype.Equals("ACHIEVER")|| archetype.Equals("achiever"))
                BartleStatistics.Instance().IncrementAchiever();
            else if (archetype.Equals("KILLER") || archetype.Equals("killer"))
                BartleStatistics.Instance().IncrementKiller();
            else if (archetype.Equals("EXPLORER") || archetype.Equals("explorer"))
                BartleStatistics.Instance().IncrementExplorer();
            else if (archetype.Equals("SOCIALIZER") || archetype.Equals("socializer"))
                BartleStatistics.Instance().IncrementSocializer();

            alreadyFired = true;
        }
    }
}
