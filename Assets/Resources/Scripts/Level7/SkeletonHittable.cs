using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHittable : Hittable
{
    [SerializeField] private Transform patrollingGuard;
    [SerializeField] private Transform[] guards;
    [SerializeField] private Transform[] skeletons;

    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        if (!GetComponent<EnemyStatus>().AIManager.enabled)
            StartAttack();
    }

    private void StartAttack()
    {
        BartleStatistics.Instance().IncrementKiller();

        foreach (Transform t in skeletons)
            t.GetComponent<EnemyStatus>().AIManager.enabled = true;

        foreach (Transform t in guards)
            t.gameObject.SetActive(true);

        Destroy(patrollingGuard.gameObject);
    }

}