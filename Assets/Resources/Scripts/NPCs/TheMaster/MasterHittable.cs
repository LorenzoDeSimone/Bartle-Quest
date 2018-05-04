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


    private bool skeletonSummoned = false, floorFallen = false , skeletonKilled = false;

    public override void UpdateHealth(int deltaHealth)
    {
        base.UpdateHealth(deltaHealth);
        float healthPerc = ((float) CurrentHealth) / ((float)MaxHealth);
        Debug.Log(healthPerc);
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
}
