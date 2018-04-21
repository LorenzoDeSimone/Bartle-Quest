using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int enemyToKill, maxEnemiesAlive, minSec = 1, maxSec = 3;

    [SerializeField] private Transform player, helperNpc;

    private int enemiesAlive, enemiesKilled;
    
    void Start()
    {
        StartCoroutine(TimedSpawner());
    }

    private IEnumerator TimedSpawner()
    {
        while (enemiesKilled + enemiesAlive < enemyToKill)
        {
            SummonEnemies();
            yield return new WaitForSeconds(Random.Range(minSec, maxSec));
        }
    }

    public void NotifyEnemyKill()
    {
        enemiesAlive--;
        enemiesKilled++;
    }

    private void SummonEnemies()
    {
        Vector3 currentDirection = Vector3.right;

        int skeletonToSummon = maxEnemiesAlive - enemiesAlive;

        for (int i = 0; i < skeletonToSummon; i++)
        {
            UnityEngine.Object Skeleton;
            if (Random.Range(0f, 1f) < 0.5f)
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonHammer");
            else
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonSword");

            GameObject skeletonGO = (GameObject)Instantiate(Skeleton);
            GuardStatus skeletonStatus = skeletonGO.GetComponent<GuardStatus>();
            NavMeshAgent agent = skeletonStatus.GetComponent<NavMeshAgent>();

            if (PlayerChoices.Instance().HelpedSpikeWithoutReward && Random.Range(0f, 1f) < 0.5f)
                skeletonStatus.target = helperNpc;
            else
                skeletonStatus.target = player;

            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position +
                                                currentDirection * agent.stoppingDistance;

            agent.Warp(spawnPosition);
            skeletonStatus.GetComponent<SkeletonDeathNotifier>().SkeletonSpawner = this;

            UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffect");
            GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
            spawnEffectGO.transform.position = skeletonStatus.transform.position;

            currentDirection = Quaternion.Euler(0f, 90f, 0f) * currentDirection;
            enemiesAlive++;
        }
    }
}
