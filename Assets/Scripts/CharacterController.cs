using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStatus))]

public class CharacterController : MonoBehaviour
{
    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    private Rigidbody myRigidbody;
    private CharacterStatus myCharacterStatus;

    private float forwardInput, turnInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    // Use this for initialization
    void Start ()
    {
        targetRotation = transform.rotation;
        myRigidbody = GetComponent<Rigidbody>();
        myCharacterStatus = GetComponent<CharacterStatus>();
        forwardInput = turnInput = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        Turn();
        Attack();
	}

    void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            myRigidbody.velocity = transform.forward * forwardInput * forwardVel;
            myCharacterStatus.RunningStatus = true;
        }
        else
        {
            myRigidbody.velocity = Vector3.zero;
            myCharacterStatus.RunningStatus = false;
        }
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);

        transform.rotation = targetRotation;
    }

    void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            myCharacterStatus.RequestAttack();
        }
            //else if (Input.GetButtonUp("Attack"))
        //    myCharacterStatus.AttackingStatus = false;
    }
}
