using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    [SerializeField]
    private State startingState;
    [SerializeField]
    private State remainInState;

    public Transform chaseTarget;

    private State currentState;

    public EnemyStats enemyStats;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    public List<Transform> wayPointList;
    [HideInInspector]
    public int nextWayPoint;

    [HideInInspector]
    public float stateTimeElapsed;

    private bool aiActive = true;

    void Start()
    {
        currentState = startingState;
    }

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetupAI(bool aiActivationFromTankManager, List<Transform> wayPointsFromTankManager)
    {
        wayPointList = wayPointsFromTankManager;
        aiActive = aiActivationFromTankManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    
    /*void OnDrawGizmos()
    {
        if (currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, enemyStats.lookSphereCastRadius);
        }
    }*/

    public void TransitionToState(State nextState)
    {
        if (!nextState.Equals(remainInState))
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }
}