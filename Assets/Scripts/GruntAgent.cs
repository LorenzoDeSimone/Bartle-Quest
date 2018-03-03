using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntAgent : MonoBehaviour
{
    [SerializeField]
    Transform destination;
    NavMeshAgent navMeshAgent;
    private CharacterStatus myCharacterStatus;

    // Use this for initialization
    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        myCharacterStatus = GetComponent<CharacterStatus>();

        if (navMeshAgent == null)
            Debug.LogError("No nav mesh agent!");
        else
            SetDestination();
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
        if (myCharacterStatus.DeathStatus)
            return;

        SetDestination();
        //myCharacterStatus.RequestAttack();
    }
}
