using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadTeleport : LordOfTheDeadsState
{

    private Transform GetRandomPoint()
    {
        return myStatus.teleportPoints[Random.Range(0, myStatus.teleportPoints.Length - 1)];
    }

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myStatus.transform.position = GetRandomPoint().position;
        myFSM.SetTrigger("teleportDone");
        CheckTransitions();
	}
}
