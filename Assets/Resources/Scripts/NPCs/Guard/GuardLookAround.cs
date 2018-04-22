using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLookAround : GuardState
{
    private float elapsedTime;
    private Quaternion currentTargetRotation;
    private Vector3[] lookAroundRandomDirections = new Vector3[2];

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        CalculateRandomDirections();
        elapsedTime = 0f;
        myGuardStatus.MovingStatus = CharacterStatus.movingRunValue;
        navMeshAgent.speed = myGuardStatus.runSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;
        LookAround();
        CheckTransitions();
    }

    private void CalculateRandomDirections()
    {
            float randValue = Random.Range(0f, 1f);
        //Debug.Log(randValue);     
        if (randValue <= 0.5f)
            lookAroundRandomDirections[0] = -myGuardStatus.transform.right;
        else
            lookAroundRandomDirections[0] = myGuardStatus.transform.right;

        lookAroundRandomDirections[1] = myGuardStatus.transform.forward;

    }

    private void LookAround()
    {
        //Debug.Log(elapsedTime / myGuardStatus.lookAroundTime);
        float percentageElapsedTime = elapsedTime / myGuardStatus.lookAroundTime;

        //CHEATING: goes a little bit closer to true player position
        if (percentageElapsedTime <= 0.2f)
        {
            navMeshAgent.destination = myGuardStatus.Target.position;
            navMeshAgent.isStopped = false;
        }
        else
        {
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            //Debug.Log(lookAroundRandomDirections);
            navMeshAgent.isStopped = true;
            if (percentageElapsedTime <= 0.6f)
                RotateTowards(myGuardStatus.transform.position + lookAroundRandomDirections[0]);
            else
                RotateTowards(myGuardStatus.transform.position + lookAroundRandomDirections[1]);
        }
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        if (elapsedTime>= myGuardStatus.lookAroundTime)
            myFSM.SetTrigger("lookAroundDone");

        if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.Target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
    }
}
