using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStatus : CharacterStatus
{
    [SerializeField] public Transform target;
    [SerializeField] public Transform[] wayPoints;
    [HideInInspector] public int nextWayPoint;

    // Use this for initialization
    protected new void Start ()
    {
        base.Start();
	}
}
