using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KidGhost : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    [SerializeField] private Transform target;
    [SerializeField] private PlayerCandle playerCandle;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > navMeshAgent.stoppingDistance)
        {
            navMeshAgent.destination = target.position;
            navMeshAgent.isStopped = false;
        }
        else
            navMeshAgent.isStopped = true;

        animator.SetBool("isMoving", !navMeshAgent.isStopped);
    }

    public void GivePlayerCandle()
    {
        playerCandle.gameObject.SetActive(true);
    }
}
