using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class GuardState : State
{
    [HideInInspector] protected NavMeshAgent navMeshAgent;
    [HideInInspector] protected GuardStatus myGuardStatus;
    [HideInInspector] protected Animator myFSM;
    [HideInInspector] private bool initDone = false;

    protected override abstract void CheckTransitions();

    protected void Initialization(Animator animator)
    {
        if (!initDone)
        {
            initDone = true;
            navMeshAgent = animator.GetComponentInParent<NavMeshAgent>();
            myGuardStatus = animator.GetComponentInParent<GuardStatus>();
            myFSM = animator;
        }
    }

    //List of utility functions to be used during transition checks
    protected bool IsPlayerInSight()
    {
        if (!initDone)
            return false;

        return Vector3.Distance(myGuardStatus.transform.position, myGuardStatus.target.position) < 2f;
    }

}
