using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsState : State
{
    [HideInInspector] protected LordOfTheDeadsStatus myStatus;
    [HideInInspector] protected Hittable myHittable;
    [HideInInspector] protected Animator myFSM;
    [HideInInspector] private bool initDone = false;
    protected float stoppingDistance = 4f;
    protected int lastHealth = 0;

    protected void Initialization(Animator animator)
    {
        if (!initDone)
        {
            initDone = true;
            myStatus = animator.GetComponentInParent<LordOfTheDeadsStatus>();
            myHittable = animator.GetComponentInParent<Hittable>();
            myFSM = animator;
        }
    }


    protected void RotateTowards(Vector3 target)
    {
        Vector3 sameYTarget = new Vector3(target.x, myStatus.transform.position.y, target.z);
        Quaternion targetRotation = Quaternion.LookRotation(sameYTarget - myStatus.transform.position, Vector3.up);
        myStatus.transform.rotation = Quaternion.Slerp(myStatus.transform.rotation, targetRotation, myStatus.turnSpeed * Time.deltaTime);
    }

    protected override void CheckTransitions()
    {
        lastHealth = myHittable.CurrentHealth;

        if (myHittable.JustHit)
        {
            myFSM.SetTrigger("justHit");
            myFSM.SetInteger("hitTaken", myFSM.GetInteger("hitTaken") + 1);
            if (myFSM.GetInteger("hitTaken") >= myStatus.deltaHitToTeleport && myStatus.teleportsDone < myStatus.maxTeleports)
            {
                myStatus.teleportsDone++;
                myFSM.SetInteger("hitTaken", 0);
                myFSM.SetTrigger("teleport");
            }
        }

        if (myStatus.DeathStatus)
        {
            myStatus.door.GetComponent<ExplodingDoor>().Explode();
            myFSM.SetTrigger("isDead");
            myStatus.friendlyGhosts.gameObject.SetActive(true);
            myStatus.friendlyGhosts.SpawnDialogue("Level2AchieverEnd");
        }
        else if (myStatus.PetrifiedStatus)
        {
            myStatus.door.GetComponent<ExplodingDoor>().Explode();
            myFSM.SetTrigger("isPetrified");
            myStatus.friendlyGhosts.gameObject.SetActive(true);
            myStatus.friendlyGhosts.SpawnDialogue("Level2ExplorerEnd");
        }
    }
}
