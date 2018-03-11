using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class GuardState : State
{
    [HideInInspector] protected NavMeshAgent navMeshAgent;
    [HideInInspector] protected GuardStatus myGuardStatus;
    [HideInInspector] protected Hittable myHittable;
    [HideInInspector] protected Animator myFSM;
    [HideInInspector] private bool initDone = false;

    protected static readonly int targetNotSeen = 0 , targetInSight = 1;

    protected void Initialization(Animator animator)
    {
        if (!initDone)
        {
            initDone = true;
            navMeshAgent = animator.GetComponentInParent<NavMeshAgent>();
            navMeshAgent.updateRotation = true;
            myGuardStatus = animator.GetComponentInParent<GuardStatus>();
            myHittable = animator.GetComponentInParent<Hittable>();
            myFSM = animator;
        }
    }

    protected void RotateTowards(Vector3 target)
    {
        Vector3 sameYTarget = new Vector3(target.x, myGuardStatus.transform.position.y, target.z);
        Quaternion targetRotation = Quaternion.LookRotation(sameYTarget - myGuardStatus.transform.position, Vector3.up);
        myGuardStatus.transform.rotation = Quaternion.Slerp(myGuardStatus.transform.rotation, targetRotation, myGuardStatus.turnSpeed * Time.deltaTime);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += myFSM.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    //List of utility functions to be used during transition checks
    protected bool IsTargetInSight(float viewRadius)
    {
        if (!initDone)
            return false;

        Vector3 viewAngleA = DirFromAngle(-myGuardStatus.viewAngle * 0.5f, false);
        Vector3 viewAngleB = DirFromAngle(myGuardStatus.viewAngle * 0.5f, false);

        Debug.DrawLine(myFSM.transform.position, myFSM.transform.position + viewAngleA * viewRadius, Color.red);
        Debug.DrawLine(myFSM.transform.position, myFSM.transform.position + viewAngleB * viewRadius, Color.red);

        float distanceToTarget = Vector3.Distance(myFSM.transform.position, myGuardStatus.target.position);
        Vector3 directionToTarget = myGuardStatus.target.position - myFSM.transform.position;
        bool wallBetween = Physics.Raycast(myFSM.transform.position, directionToTarget, distanceToTarget, LayerMask.GetMask("Wall"));

        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.target.position);
        //Debug.Log(targetColliders.Length);

        //If the target is running and is near the guard, it will chase regardless of direction
        if(distance <= myGuardStatus.distanceForInstantChase && !wallBetween && 
           myGuardStatus.target.GetComponent<CharacterStatus>().MovingStatus == CharacterStatus.movingRunValue)
            return true;
        else if (Vector3.Distance(myFSM.transform.position, myGuardStatus.target.position) <= viewRadius)
        {
            //Checks if the target is within the angle of sight
            if (Vector3.Angle(myFSM.transform.forward, directionToTarget) < myGuardStatus.viewAngle * 0.5f)
                return !wallBetween;
        }
        return false;
    }

    protected override void CheckTransitions()
    {
        if (myGuardStatus.DeathStatus)
            myFSM.SetTrigger("isDead");
        else if (myHittable.JustHit)
            myFSM.SetTrigger("justHit");
    }
}
