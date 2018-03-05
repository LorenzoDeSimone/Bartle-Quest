using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerNearby")]
public class PlayerNearbyDecision : Decision
{
    [SerializeField]
    private float closeDistance;

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        //Debug.Log(Vector3.Distance(controller.transform.position, controller.chaseTarget.position));
        if (Vector3.Distance(controller.transform.position, controller.chaseTarget.position) <  closeDistance)
            return true;
        else
            return false;
        /*    Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);
              if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.lookRange)
                  && hit.collider.CompareTag("Player"))
              {
                  controller.chaseTarget = hit.transform;
                  return true;
              }*/
    }
}