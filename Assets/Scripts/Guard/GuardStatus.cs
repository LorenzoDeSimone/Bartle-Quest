using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatus : CharacterStatus
{
    [SerializeField] public float distanceForInstantChase = 3f;
    [SerializeField] public float viewRadius = 20f;
    [SerializeField] [Range(0, 360)] public float viewAngle = 60f;
    [SerializeField] public float turnSpeed = 8f;

    [SerializeField] public Transform target;
    [SerializeField] public Transform[] wayPoints;
    [HideInInspector] public int nextWayPoint;

    // Use this for initialization
    protected new void Start ()
    {
        base.Start();
	}
}
