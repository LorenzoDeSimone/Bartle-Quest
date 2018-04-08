using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    [SerializeField] public GameObject DeathFade;
    [SerializeField] private Transform characterMotionRoot;
    [SerializeField] public Transform target;
    [SerializeField] public float timeToDisappearAfterDeath = 1.5f;
    [SerializeField] public float walkSpeed = 2f;
    [SerializeField] public float runSpeed = 4f;
    [SerializeField] public float turnSpeed = 5f;

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        HealthBar.CreateHealthBar(characterMotionRoot, GetComponent<Hittable>());
    }
}
