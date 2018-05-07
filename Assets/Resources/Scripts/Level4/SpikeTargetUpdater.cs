using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTargetUpdater : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    private GuardStatus guardStatus;

    void Start()
    {
        if (!PlayerChoices.Instance().HelpedSpikeWithoutReward)
        {
            enemySpawner.RemoveHelper();
            Destroy(gameObject);
        }
        guardStatus = GetComponent<GuardStatus>();
    }

    void Update()
    {
        guardStatus.Target = enemySpawner.GetNearestEnemy(transform.position);
    }
}
