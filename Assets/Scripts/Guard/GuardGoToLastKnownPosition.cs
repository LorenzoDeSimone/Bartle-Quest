using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GuardGoToLastKnownPosition : GuardState
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myGuardStatus.MovingStatus = CharacterStatus.movingRunValue;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //RotateTowards(myGuardStatus.target.position);
        navMeshAgent.destination = myGuardStatus.lastTargetPosition;
        navMeshAgent.isStopped = false;

        CheckTransitions();
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
        {
            bool lastKnownTargetPositionReached = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending;

            if (lastKnownTargetPositionReached)
                myFSM.SetTrigger("lastKnownPositionReached");
        }
    }
}
