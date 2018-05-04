using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStatus : GuardStatus
{
    [SerializeField] private EnemySpawner skeletonSpawner;
    [SerializeField] private string canSpeakWithSkeletonDialogue, cannotSpeakWithSkeletonDialogue;

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        if (PlayerChoices.Instance().CanSpeakWithSkeletons)
            GetComponent<Talker>().DialogueName = canSpeakWithSkeletonDialogue;
        else
            GetComponent<Talker>().DialogueName = cannotSpeakWithSkeletonDialogue;
    }

    public bool FlyingStatus
    {
        set
        {
            if (value)
            {
                myAnimator.SetTrigger("fly");
                AIManager.SetTrigger("fly");
            }
            else
            {
                myAnimator.SetTrigger("flyEnd");
                AIManager.SetTrigger("flyEnd");
            }
        }
    }

    public bool RoarStatus
    {
        set
        {
            if (value)
                myAnimator.SetTrigger("roar");
        }
    }

}
