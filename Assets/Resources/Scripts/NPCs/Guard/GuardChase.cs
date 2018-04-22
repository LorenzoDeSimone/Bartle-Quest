using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GuardChase : GuardState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myGuardStatus.MovingStatus = CharacterStatus.movingRunValue;
        navMeshAgent.speed = myGuardStatus.runSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        //RotateTowards(myGuardStatus.target.position);
        //if(myGuardStatus.target == null)

        navMeshAgent.destination = myGuardStatus.Target.position;

        navMeshAgent.isStopped = false;
        CheckTransitions();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.isStopped = true;
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.Target.position);

        if (distance <= myGuardStatus.attackRadius)
            myFSM.SetBool("fighting", true);

        if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.Target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
        {
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
        }
    }
}
