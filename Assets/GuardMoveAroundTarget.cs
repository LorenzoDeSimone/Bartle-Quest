using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMoveAroundTarget : GuardState
{
    Vector3 offsetFromTargetToReach;
    float elapsedTime = 0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        float utilityRandom = Random.Range(0f, 1f);

        //Percentage of going nowhere around character
        if (utilityRandom <= 0.5f)
        {
            //Debug.Log("I stay and fight");
            myFSM.SetTrigger("moveAroundTargetDone");
        }
        else
        {
            //Debug.Log("I go back a little");
            //Same number used to get a random distance for the agent to go back
            utilityRandom = 1 + utilityRandom * 0.5f;

            myGuardStatus.MovingStatus = CharacterStatus.movingWalkValue;
            navMeshAgent.speed = myGuardStatus.walkSpeed;
            navMeshAgent.destination = myGuardStatus.transform.position - myGuardStatus.transform.forward * navMeshAgent.stoppingDistance * utilityRandom;

            navMeshAgent.isStopped = false;
            navMeshAgent.updateRotation = false;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        elapsedTime += Time.deltaTime;
        Debug.DrawLine(myGuardStatus.transform.position, navMeshAgent.destination, Color.red);
        //navMeshAgent.destination = myGuardStatus.target.position + offsetFromTargetToReach;
        CheckTransitions();
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            navMeshAgent.updateRotation = true;

            if (elapsedTime > 3f)
                myFSM.SetTrigger("moveAroundTargetDone");
        }
    }
}
