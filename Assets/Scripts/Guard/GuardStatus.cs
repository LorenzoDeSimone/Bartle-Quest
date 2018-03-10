using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatus : CharacterStatus
{
    [SerializeField] public float attackRadius = 5f;
    [SerializeField] public float distanceForInstantChase = 5f;
    [SerializeField] public float patrolViewRadius = 15f;
    [SerializeField] public float chaseViewRadius = 20f;

    [SerializeField] public float walkSpeed = 2f;
    [SerializeField] public float runSpeed = 4f;

    [SerializeField] public float waypointWaitMin = 1f;
    [SerializeField] public float waypointWaitMax = 5f;

    [SerializeField] [Range(0, 360)] public float viewAngle = 60f;
    [SerializeField] public float turnSpeed = 5f;
    [SerializeField] public float lookAroundTime = 5f;
    [SerializeField] public Transform target;
    [SerializeField] public Transform[] wayPoints;
    [HideInInspector] public int nextWayPoint;
    [HideInInspector] public Vector3 lastTargetPosition;
    [HideInInspector] public float timeToDisappearAfterDeath = 3f;


    // Use this for initialization
    protected new void Start ()
    {
        base.Start();
	}
}
