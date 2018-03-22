using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [SerializeField] private Transform NPC;
    [SerializeField] private Transform NPCFinalTransform;
    [SerializeField] private string dialogueName;

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
        {
            NPC.position = NPCFinalTransform.position;
            NPC.rotation = NPCFinalTransform.rotation;
            NPC.GetComponent<Talker>().enabled = false;
            NPC.GetComponent<Target>().enabled = false;

            BartleStatistics.Instance().IncrementExplorer();
        }
    }
}
