using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MasterHittable : Hittable
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform floorToDestroy;
    [SerializeField] private EnemySpawner skeletonSpawner;
    [SerializeField] private float skeletonSummonPerc, floorFallPerc;
    [SerializeField] private string finalMasterDialogue;


    private bool skeletonSummoned = false, floorFallen = false , skeletonKilled = false;

    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        float healthPerc = ((float) CurrentHealth) / ((float)MaxHealth);

        if (healthPerc <= skeletonSummonPerc && !skeletonSummoned)
        {
            skeletonSummoned = true;
            skeletonSpawner.StartEnemySpawning();
            FlightStatus(true);
            dialogueManager.InitDialogue(GetComponent<Talker>());
        }
        else if(healthPerc < floorFallPerc && !floorFallen)
        {
            floorFallen = true;
            BringFloorDown();
            GetComponent<MasterStatus>().RoarStatus = true;
        }

        if(CurrentHealth <= 0)
            CallDeathDialogue();
    }

    protected override void Update()
    {
        base.Update();
        if(skeletonSpawner.FightEnded && !skeletonKilled)
        {
            skeletonKilled = true;
            FlightStatus(false);
        }
    }

    private void FlightStatus(bool value)
    {
        GetComponent<MasterStatus>().FlyingStatus = value;
        GetComponent<Target>().enabled = !value;
        GetComponent<NavMeshAgent>().enabled = !value;
        GetComponent<Collider>().enabled = !value;
    }

    private void BringFloorDown()
    {
        FallingFloor[] floors = floorToDestroy.GetComponentsInChildren <FallingFloor>();
        foreach(FallingFloor floor in floors)
            floor.Fall();
    }

    private void CallDeathDialogue()
    {
        Talker talker = GetComponent<Talker>();
        talker.DialogueName = finalMasterDialogue;
        dialogueManager.InitDialogue(talker);
    }
}
