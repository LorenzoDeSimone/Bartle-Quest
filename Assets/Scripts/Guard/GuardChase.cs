using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GuardChase : GuardState
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);

        Rotate();
        navMeshAgent.destination = myGuardStatus.target.position;
        navMeshAgent.isStopped = false;

        CheckTransitions();
    }

    protected override void CheckTransitions()
    {
        myFSM.SetBool("playerInSight", IsTargetInSight());
    }
}
