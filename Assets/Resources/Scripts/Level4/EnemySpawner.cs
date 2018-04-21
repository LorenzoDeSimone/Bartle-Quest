using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private int enemyToKill, maxEnemiesAlive, minSec = 1, maxSec = 3;

    [SerializeField] private Transform player, helperNpc;

    private HashSet<Transform> enemiesAlive;
    private int enemiesKilled;
    
    void Awake()
    {
        enemiesAlive = new HashSet<Transform>();
    }

    public void StartEnemySpawning()
    {
        StartCoroutine(TimedSpawner());
    }

    public Transform GetNearestTarget(Vector3 position)
    {
        float closestDist = Mathf.Infinity;
        Transform closestEnemy = null;
        float dist;

        foreach(Transform currEnemy in enemiesAlive)
        {
            dist = Vector3.Distance(currEnemy.position, position);
            if(dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = currEnemy;
            }
        }

        return closestEnemy;
    }

    private IEnumerator TimedSpawner()
    {
        while (enemiesKilled + enemiesAlive.Count < enemyToKill)
        {
            SummonEnemies();
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSec, maxSec));
        }
    }

    public void NotifyEnemyKill(Transform enemy)
    {
        enemiesAlive.Remove(enemy);
        enemiesKilled++;
    }

    private void SummonEnemies()
    {
        Vector3 currentDirection = Vector3.right;

        int skeletonToSummon = maxEnemiesAlive - enemiesAlive.Count;

        for (int i = 0; i < skeletonToSummon; i++)
        {
            UnityEngine.Object Skeleton;
            if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonHammer");
            else
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonSword");

            GameObject skeletonGO = (GameObject)Instantiate(Skeleton);
            GuardStatus skeletonStatus = skeletonGO.GetComponent<GuardStatus>();
            NavMeshAgent agent = skeletonStatus.GetComponent<NavMeshAgent>();
            
            if (PlayerChoices.Instance().HelpedSpikeWithoutReward && UnityEngine.Random.Range(0f, 1f) < 0.5f)
                skeletonStatus.target = helperNpc;
            else
                skeletonStatus.target = player;

            Vector3 spawnPosition = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)].position +
                                                currentDirection * agent.stoppingDistance;

            agent.Warp(spawnPosition);
            skeletonStatus.GetComponent<SkeletonDeathNotifier>().SkeletonSpawner = this;

            UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffectGrey");
            GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
            spawnEffectGO.transform.position = skeletonStatus.transform.position;

            currentDirection = Quaternion.Euler(0f, 90f, 0f) * currentDirection;
            enemiesAlive.Add(skeletonStatus.transform);
        }
    }
}
