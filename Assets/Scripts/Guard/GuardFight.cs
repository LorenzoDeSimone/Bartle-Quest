using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFight : GuardState
{
    float timefromLastAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
        timefromLastAttack = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RotateTowards(myGuardStatus.target.position);

        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.target.position);

        if (distance <= navMeshAgent.stoppingDistance && !myGuardStatus.DeathStatus)
        {
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            myGuardStatus.RequestAttack();
        }
        else
        {
            navMeshAgent.destination = myGuardStatus.target.position;
            navMeshAgent.isStopped = false;
            myGuardStatus.MovingStatus = CharacterStatus.movingRunValue;
        }

        CheckTransitions();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}f

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.target.position);

        if (distance > myGuardStatus.attackRadius)
            myFSM.SetBool("fighting", false);

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
