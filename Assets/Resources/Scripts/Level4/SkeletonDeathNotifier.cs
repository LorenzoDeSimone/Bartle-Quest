using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathNotifier : MonoBehaviour
{
    [SerializeField] private EnemySpawner skeletonSpawner;

    public EnemySpawner SkeletonSpawner
    {
        set { skeletonSpawner = value; }
    }

    void OnDestroy()
    {
        if(skeletonSpawner)
            skeletonSpawner.NotifyEnemyKill(transform);
    }
}
