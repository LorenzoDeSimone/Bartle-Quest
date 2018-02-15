using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStatus))]

public class MovingCharacter : MonoBehaviour
{
    [SerializeField] private float inputDelay = 0.1f;
    //[SerializeField] private float jumpForce = 10f;
    [SerializeField] private float raycastLenght = 20f;

    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    private Rigidbody myRigidbody;
    private CharacterStatus myCharacterStatus;
    private Collider myCollider;

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
        myCollider = GetComponent<Collider>();
	}

    private bool IsBorderOK(float forwardInput)
    {
        Vector3 nextPos = myCollider.bounds.center + transform.forward * forwardInput;

        //Checks to avoid falling into the abyss
        if (Physics.Raycast(nextPos, -transform.up, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            Vector3 firstRay  = (transform.forward + transform.forward + transform.right) * forwardInput;
            Vector3 secondRay = (transform.forward + transform.forward - transform.right) * forwardInput;

            //Check to avoid running into the walls
            if (Physics.Raycast(myCollider.bounds.center, firstRay, (myCollider.bounds.center - nextPos).magnitude) ||
                Physics.Raycast(myCollider.bounds.center, secondRay, (myCollider.bounds.center - nextPos).magnitude))
                return false;
            else
                return true;
        }
        else
            return false;
    }

    public void Run(float forwardInput)
    {
        if (Mathf.Abs(forwardInput) > inputDelay && !myCharacterStatus.AttackingStatus && IsBorderOK(forwardInput))
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

    /*public void Jump()
    {
        if(Physics.Raycast(transform.position, Vector3.down, raycastLenght, LayerMask.GetMask("Ground")))
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }*/

    void Update()
    {
        Vector3 nextPos = myCollider.bounds.center + transform.forward;
        //Debug.DrawLine(myCollider.bounds.center, nextPos, Color.red);
        //Debug.DrawRay(nextPos, Vector3.down * 10, Color.red);
        //Debug.DrawRay(myCollider.bounds.center, transform.forward * (myCollider.bounds.center - nextPos).magnitude, Color.red);
        //Physics.Raycast(myCollider.bounds.center, transform.forward, (myCollider.bounds.center + nextPos).magnitude);
        //Debug.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght, Color.red);
        Vector3 firstRay = (transform.forward + transform.forward + transform.right);
        Vector3 secondRay = (transform.forward + transform.forward - transform.right);
        Debug.DrawRay(myCollider.bounds.center, firstRay, Color.red);
        Debug.DrawRay(myCollider.bounds.center, secondRay, Color.red);
    }
}

