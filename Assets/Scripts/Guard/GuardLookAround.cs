using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardLookAround : GuardState
{
    private float elapsedTime;
    private Quaternion currentTargetRotation;
    private Vector3[] lookAroundRandomDirections = new Vector3[3];

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        CalculateRandomDirections();
        elapsedTime = 0f;
        myGuardStatus.MovingStatus = CharacterStatus.movingWalkValue;

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
        for (int i = 0; i < 3; i++)
        {
            float randValue = Random.Range(0f, 1f);
            //Debug.Log(randValue);
            if (randValue <= 0.3333f)
            {
                Vector3 newDir = new Vector3(-myGuardStatus.transform.forward.x, -myGuardStatus.transform.forward.y, -myGuardStatus.transform.forward.z);
                lookAroundRandomDirections[i] = newDir;
            }         
            else if (randValue <= 0.6666f)
                lookAroundRandomDirections[i] = -myGuardStatus.transform.right;
            else
                lookAroundRandomDirections[i] = myGuardStatus.transform.right;
        }
    }

    private void LookAround()
    {
        //Debug.Log(elapsedTime / myGuardStatus.lookAroundTime);
        float percentageElapsedTime = elapsedTime / myGuardStatus.lookAroundTime;

        //CHEATING: goes a little bit closer to true player position
        if (percentageElapsedTime <= 0.2f)
        {
            navMeshAgent.destination = myGuardStatus.target.position;
            navMeshAgent.isStopped = false;
        }
        else
        {
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            //Debug.Log(lookAroundRandomDirections);
            navMeshAgent.isStopped = true;
            if (percentageElapsedTime <= 0.50f)
                RotateTowards(myGuardStatus.transform.position + lookAroundRandomDirections[0]);
            else if (percentageElapsedTime <= 0.75f)
                RotateTowards(myGuardStatus.transform.position + lookAroundRandomDirections[1]);
            else
                RotateTowards(myGuardStatus.transform.position + lookAroundRandomDirections[2]);
        }
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        if (elapsedTime>= myGuardStatus.lookAroundTime)
            myFSM.SetTrigger("lookAroundDone");

        if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
    }
}
