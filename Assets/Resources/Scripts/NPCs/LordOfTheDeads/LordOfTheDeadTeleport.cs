using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class LordOfTheDeadTeleport : LordOfTheDeadsState
{
    private int GetRandomIndex()
    {
        return Random.Range(0, myStatus.teleportPoints.Length - 1);
    }

    private void SummonSkeletons()
    {
        Vector3 currentDirection = myStatus.target.transform.forward;

        for (int i = 0; i < myStatus.skeletonToSummon; i++)
        {
            UnityEngine.Object Skeleton;
            if (Random.Range(0f, 1f) < 0.5f)
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonHammerSlave");
            else
                Skeleton = Resources.Load("Prefabs/NPCs/Skeleton/SkeletonSwordSlave");

            GameObject skeletonGO = (GameObject)Instantiate(Skeleton);
            GuardStatus skeletonStatus = skeletonGO.GetComponent<GuardStatus>();
            skeletonGO.GetComponent<DeathNotifier>().SetFloatingSkull(myStatus.floatingSkullLights[i]);
            LitFloatingSkull(i);

            skeletonStatus.target = myStatus.target;
            skeletonStatus.transform.position = myStatus.target.transform.position +
                                                currentDirection * skeletonStatus.GetComponent<NavMeshAgent>().stoppingDistance;

            UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffect");
            GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
            spawnEffectGO.transform.position = skeletonStatus.transform.position;

            currentDirection = Quaternion.Euler(0f, 90f, 0f) * currentDirection;
        }
    }

    private void LitFloatingSkull(int i)
    {
        myStatus.floatingSkullLights[i].gameObject.SetActive(true);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        SummonSkeletons();
        myStatus.skeletonToSummon++;
        int randomIndex = GetRandomIndex();

        UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffect");
        GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
        spawnEffectGO.transform.position = myStatus.transform.position;

        myStatus.transform.position = myStatus.teleportPoints[randomIndex].position;
        spawnEffectGO = (GameObject)Instantiate(spawnEffect);
        spawnEffectGO.transform.position = myStatus.transform.position;

        myStatus.GetComponent<Hittable>().Invincible = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RotateTowards(myStatus.target.position);
        CheckTransitions();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myStatus.GetComponent<Hittable>().Invincible = false;
    }

    private bool FloatingSkullsLeft()
    {
        foreach(ParticleSystem t in myStatus.floatingSkullLights)
        {
            if (t.gameObject.active)
                return true;
        }
        return false;
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();
        if(!FloatingSkullsLeft())
            myFSM.SetTrigger("teleportDone");
    }
}
