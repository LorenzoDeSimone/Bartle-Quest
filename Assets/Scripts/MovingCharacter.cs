using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStatus))]

public class MovingCharacter : MonoBehaviour
{
    [SerializeField] private float inputDelay = 0.1f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float raycastLenght = 20f;

    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    private Rigidbody myRigidbody;
    private CharacterStatus myCharacterStatus;


    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    // Use this for initialization
    void Start ()
    {
        targetRotation = transform.rotation;
        myRigidbody = GetComponent<Rigidbody>();
        myCharacterStatus = GetComponent<CharacterStatus>();
	}

    public void Run(float forwardInput)
    {
        if (Mathf.Abs(forwardInput) > inputDelay && !myCharacterStatus.AttackingStatus)
        {
            transform.position += transform.forward * forwardInput * forwardVel * Time.deltaTime;
            myCharacterStatus.RunningStatus = true;
        }
        else
            myCharacterStatus.RunningStatus = false;
    }

    public void Turn(float turnInput)
    {
        if (Mathf.Abs(turnInput) > inputDelay)
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);

        transform.rotation = targetRotation;
    }

    public void Attack()
    {
        myCharacterStatus.RequestAttack();
    }

    public void Jump()
    {
        if(Physics.Raycast(transform.position, Vector3.down, raycastLenght, LayerMask.GetMask("Ground")))
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght, Color.red);
    }
}

