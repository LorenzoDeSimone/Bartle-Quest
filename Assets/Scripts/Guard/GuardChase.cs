﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GuardChase : GuardState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        RotateTowards(myGuardStatus.target.position);
        navMeshAgent.destination = myGuardStatus.target.position;
        navMeshAgent.isStopped = false;

        CheckTransitions();
    }

    protected override void CheckTransitions()
    {
        if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
        {
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
        }
    }
}
