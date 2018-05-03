using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHittable : Hittable
{
    [SerializeField] private Transform ghostPositions;
    [SerializeField] private Target talkingHeads;

    private static HashSet<Transform> ghosts;
    private static bool someoneAttacked;
    private static int ghostsKilled, ghostsToKill;

    protected new void Start()
    {
        base.Start();
        if (ghosts == null || ghosts.Count == 0)
        {
            ghostsKilled = 0;
            ghostsToKill = 0;
            someoneAttacked = false;
            ghosts = new HashSet<Transform>();
        }

        ghostsToKill++;
        ghosts.Add(transform);
    }

    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        if(!someoneAttacked)
        {
            someoneAttacked = true;
            StartAttack();
        }

        if (currentHealth <= 0)
        {
            ghostsKilled++;
            if (ghostsKilled == ghostsToKill && talkingHeads)
            {
                talkingHeads.gameObject.SetActive(true);
            }
        }
    }

    private void StartAttack()
    {
        Destroy(ghostPositions.gameObject);
        BartleStatistics.Instance().IncrementKiller();

        foreach (Transform t in ghosts)
            t.GetComponent<EnemyStatus>().AIManager.enabled = true;
    }

    void OnDestroy()
    {
        ghosts.Remove(transform);
    }
}
