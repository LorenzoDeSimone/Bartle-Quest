using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDeath : GuardState
{
    private float elapsedTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        navMeshAgent.isStopped = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= myGuardStatus.timeToDisappearAfterDeath)
        {
            myGuardStatus.DeathFade.SetActive(true);
            myGuardStatus.DeathFade.transform.parent = null;
            Destroy(myGuardStatus.gameObject);
        }
        //gameObject.GetComponent<MeshRenderer>().material = myMaterial;
    }

    protected override void CheckTransitions(){}
}
