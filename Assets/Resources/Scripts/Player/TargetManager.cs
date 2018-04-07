using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private LayerMask targetUnreachableMask;
    protected HashSet<Transform> nearTargets;

    protected void Start()
    {
        nearTargets = new HashSet<Transform>();
        targetUnreachableMask = LayerMask.GetMask("Ground");
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Target>() != null)
        {
            nearTargets.Add(collider.gameObject.transform);
            //Debug.Log("IN");
        }
    }

    protected void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Target>() != null)
        {
            nearTargets.Remove(collider.gameObject.transform);
            //Debug.Log("OUT");
        }
    }

    public Transform GetNearestTarget(bool enemyOnly = false)
    {
        float minDist = Mathf.Infinity;
        Transform nearestTarget = null;

        float currDist;
        foreach (Transform t in nearTargets)
        {
            if (t != null)
            {
                Target target = t.GetComponent<Target>();
                if (!enemyOnly || (enemyOnly && target.IsEnemy))
                {
                    if (target != null && target.enabled)
                    {
                        currDist = Vector3.Distance(t.position, transform.position);
                        if (currDist < minDist)
                        {
                            //if (!Physics.Raycast(transform.position, (t.position - transform.position).normalized,
                            //currDist, targetUnreachableMask))
                            //{
                                minDist = currDist;
                                nearestTarget = t;
                            //}
                        }
                    }
                }
            }
        }

        return nearestTarget;
    }
}