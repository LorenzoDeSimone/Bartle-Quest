using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    Rigidbody rBody;
    private float forwardInput, turnInput;

    private bool isRunning = false;
    private Animator myAnimator;

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
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("Character has no rigidbody.");

        if (GetComponent <Animator>())
            myAnimator = GetComponent<Animator>();
        else
            Debug.LogError("Character has no animator.");

        forwardInput = turnInput = 0;

	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        Turn();
	}

    void FixedUpdate()
    {
        Run();
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            rBody.velocity = transform.forward * forwardInput * forwardVel;
            isRunning = true;
        }
        else
        {
            rBody.velocity = Vector3.zero;
            isRunning = false;
        }

        myAnimator.SetBool("isRunning",isRunning);
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputDelay)
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);

        transform.rotation = targetRotation;
    }
}
