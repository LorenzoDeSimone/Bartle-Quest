using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8General : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private MasterStatus master;

    private bool isHelpingInFight = false, fightStarted = false;
    private GuardStatus guardStatus;

    public bool IsHelpingInFight
    {
        get { return isHelpingInFight; }
    }

	// Use this for initialization
	void Start ()
    {
        guardStatus = GetComponent<GuardStatus>();
	}
	
    public void HelpWithFight()
    {
        isHelpingInFight = true;
        guardStatus.AIManager.enabled = true;
    }

    public void FightStarted()
    {
        if (IsHelpingInFight)
        {
            GetComponent<GuardStatus>().CanAttack = true;
            fightStarted = true;
        }
    }

    void Update()
    {
        if (fightStarted)
        {
            if (master.Equals(null))
                guardStatus.CanAttack = false;
            else if (master.GetComponent<Collider>().enabled == false)
                guardStatus.Target = enemySpawner.GetNearestEnemy(transform.position);
            else
                guardStatus.Target = master.transform;
        }
    }
}
