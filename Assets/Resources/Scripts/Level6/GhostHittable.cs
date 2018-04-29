using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHittable : Hittable
{
    [SerializeField] private Transform ghostPositions;
    [SerializeField] private Target talkingHeads;

    private static HashSet<Transform> ghosts;
    private static bool someoneAttacked = false;
    private static int ghostsKilled = 0;

    protected new void Start()
    {
        base.Start();
        if (ghosts == null)
            ghosts = new HashSet<Transform>();
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
            if (ghostsKilled == ghosts.Count && talkingHeads)
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
}
