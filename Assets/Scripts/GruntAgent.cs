using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntAgent : MonoBehaviour
{
    [SerializeField]
    Transform destination;
    NavMeshAgent navMeshAgent;
    private PlayerStatus myCharacterStatus;
    private bool atkCoroutineStarted = false;

    // Use this for initialization
    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        myCharacterStatus = GetComponent<PlayerStatus>();

        if (navMeshAgent == null)
            Debug.LogError("No nav mesh agent!");
        else
        {
            SetDestination();
            //if (myCharacterStatus != null)
            //    StartCoroutine(AttackRoutine());
            //else
            //    Debug.Log("No character status");
        }
	}
	
    private void SetDestination()
    {
        if(destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (!atkCoroutineStarted)
        {
            StartCoroutine(AttackRoutine());
            atkCoroutineStarted = true;
        }

        if (myCharacterStatus.DeathStatus)
           return;

        SetDestination();
    }

    private IEnumerator AttackRoutine()
    {
        while (!myCharacterStatus.DeathStatus)
        {
            myCharacterStatus.RequestAttack();
            yield return new WaitForSeconds(2f);
        }
    } 
}
