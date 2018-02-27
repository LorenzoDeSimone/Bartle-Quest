using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private HashSet<Transform> nearTargets;

    void Start()
    {
        nearTargets = new HashSet<Transform>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Target>() != null)
        {
            nearTargets.Add(collider.gameObject.transform);
            Debug.Log("IN");
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Target>() != null)
        {
            nearTargets.Remove(collider.gameObject.transform);
            Debug.Log("OUT");
        }
    }

    public Transform GetNearestTarget()
    {
        float minDist = Mathf.Infinity;
        Transform nearestTarget = null;

        float currDist;
        foreach (Transform t in nearTargets)
        {
            currDist = Vector3.Distance(t.position, transform.position);
            if (currDist < minDist)
            {
                minDist = currDist;
                nearestTarget = t;
            }
        }

        return nearestTarget;
    }
}
