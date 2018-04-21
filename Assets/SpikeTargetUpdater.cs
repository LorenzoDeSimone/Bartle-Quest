using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTargetUpdater : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    private GuardStatus guardStatus;

    void Start()
    {
        guardStatus = GetComponent<GuardStatus>();
        //StartCoroutine(ChangeTarget());
    }

    void Update()
    {
        guardStatus.target = enemySpawner.GetNearestTarget(transform.position);
    }

    private IEnumerator ChangeTarget()
    {
        while (enabled)
        {
            guardStatus.target = enemySpawner.GetNearestTarget(transform.position);
            yield return new WaitUntil(() => guardStatus.target == null);
        }
    }

}
