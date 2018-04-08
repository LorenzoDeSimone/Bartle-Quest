using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsFight : LordOfTheDeadsState
{
    float elapsedTime;
    float timeToWaitBeforeAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myStatus.MovingStatus = CharacterStatus.movingIdleValue;
        myFSM.SetBool("fighting", true);
        timeToWaitBeforeAttack = Random.Range(0f, 2f);
        elapsedTime = 0;
        lastHealth = myHittable.CurrentHealth;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(myFSM.transform.position, myStatus.target.position);
        //Actual fighting case
        if (distance <= stoppingDistance)
        {
            elapsedTime += Time.deltaTime;
            myStatus.MovingStatus = CharacterStatus.movingIdleValue;

            RotateTowards(myStatus.target.position);
            if (elapsedTime > timeToWaitBeforeAttack)
            {
                elapsedTime = 0f;
                myStatus.RequestAttack();
                myFSM.SetTrigger("newCombo");
            }
        }
        CheckTransitions();
    }
}
