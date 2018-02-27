using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 8-directional movement
/// 2. stop and face current direction when input is absent
/// </summary>

public class MarioController : MonoBehaviour
{
    [SerializeField] private float inputEpsilon = 0.1f;
    [SerializeField] private float runningThreshold = 0.5f;
    [SerializeField] private float velocity = 5;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float height = 0.5f;
    [SerializeField] private float heightPadding = 0.05f;
    [SerializeField] private float wallCheckLength = 1f;
    [SerializeField] private LayerMask walkableLayerMask;
    [SerializeField] private float maxGroundAngle = 120;

    [SerializeField] private bool debug;
    [SerializeField] private Transform playerModelTransform;


    private bool grounded;
    private bool fallHitSet;
    private static readonly int idleValue = 0, walkingValue = 1, runningValue = 2;
    private float angle;
    private float groundAngle;
    private Vector2 input;
    private Vector3 forward;
    private Vector3 fallVelocity;
    private Quaternion targetRotation;
    private Transform myCamera;
    private RaycastHit hitInfo, fallHitInfo;
    private CharacterStatus myCharacterStatus;
    private Cam myCameraScript;

    void Start()
    {
        myCamera = Camera.main.transform;
        walkableLayerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Stairs");
        myCharacterStatus = GetComponent<CharacterStatus>();
        myCameraScript = myCamera.GetComponent<Cam>();
    }

    void Update()
    {
        GetInput();
        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();

        Rotate();
        //if(myCameraScript.IsInLockTargetStatus())
        //    TargetLockMove();
        //else
            Move();
 
    }

    /// <summary>
    /// Input based on Horizontal (a,d,<,>) and Vertical (w,s,^,v) keys
    /// </summary>
    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Attack"))
            Attack();
    }

    /// <summary>
    /// Direction relative to the camera's rotation
    /// </summary>
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += myCamera.eulerAngles.y;
    }

    /// <summary>
    /// Rotate toward the calculated angle
    /// </summary>
    void Rotate()
    {
        if (myCameraScript.IsInLockTargetStatus())
        {
            Vector3 sameYTarget = new Vector3(myCameraScript.GetTarget().position.x, transform.position.y, myCameraScript.GetTarget().position.z);
            targetRotation = Quaternion.LookRotation(sameYTarget - transform.position, Vector3.up);
        }
        else
        {
            if (Mathf.Abs(input.x) < inputEpsilon && Mathf.Abs(input.y) < inputEpsilon)
                return;

            if (myCharacterStatus.AttackingStatus)
                return;

            targetRotation = Quaternion.Euler(0, angle, 0);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void TargetLockMove()
    {

        float vel = 0f;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            vel = input.x;
        }
        else
            vel = input.y;

        int sign = 1;

        if (vel < 0)
            sign = -1;

        if (Mathf.Abs(input.x) < inputEpsilon && Mathf.Abs(input.y) < inputEpsilon)
        {
            myCharacterStatus.MovingStatus = idleValue;
        }
        else
        {
            myCharacterStatus.MovingStatus = runningValue;

            transform.position += forward * velocity * sign * Time.deltaTime;
        }
    }

    /// <summary>
    /// This player only moves along its own forward axis
    /// </summary>
    void Move()
    {

        //Debug.Log(Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)));

        if (groundAngle > maxGroundAngle || myCharacterStatus.AttackingStatus || !IsBorderOK())
        {
            myCharacterStatus.MovingStatus = idleValue;
            return;
        }

        if (Mathf.Abs(input.x) < inputEpsilon && Mathf.Abs(input.y) < inputEpsilon)
        {
            myCharacterStatus.MovingStatus = idleValue;
        }
        else if (Mathf.Abs(input.x) < runningThreshold && Mathf.Abs(input.y) < runningThreshold)
        {
            myCharacterStatus.MovingStatus = walkingValue;
            transform.position += forward * velocity * Mathf.Max(Mathf.Abs(input.x),Mathf.Abs(input.y)) * Time.deltaTime;
        }
        else
        {
            myCharacterStatus.MovingStatus = runningValue;
            transform.position += forward * velocity * Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)) * Time.deltaTime;
        }
    }

    private Vector3 GetForwardFromInputVector()
    {
        Vector3 inputVector = (myCamera.transform.TransformDirection(input)).normalized;
        float sumXZ = inputVector.y + inputVector.z;

        inputVector.y = 0f;
        inputVector.z = 0f;
        

        return inputVector;
    }

    /// <summary>
    /// If the player is not grounded, forward will be equal to transform forward
    /// Use a cross product to determine the new forward vector
    /// </summary>
    void CalculateForward()
    {

        if(myCameraScript.IsInLockTargetStatus())
        {
            //Vector3 inputVector = new Vector3(input.x, 0f, input.y);
            Vector3 inputVector;
            //if(myCamera.transform.rotation.x < myCameraScript.yMaxLimit * 0.5f)
            inputVector = (myCamera.transform.TransformDirection(input)).normalized;
            Debug.Log(inputVector.y + inputVector.z);
            //else
            //    inputVector = (myCamera.transform.TransformDirection(input.x, 0f, input.y)).normalized;
            //inputVector.y = 0f;

            //Debug.DrawRay(hitInfo.point     , hitInfo.normal, Color.green);
            //Debug.Log(inputVector);
            Debug.DrawRay(transform.position, inputVector * 2, Color.red);
            //Debug.Log(Vector3.Cross(myCamera.forward, new Vector3(input.x, 0f, input.y)));
            //Debug.Log(Vector3.Dot(myCamera.forward, -transform.right));
            //forward = new Vector3(inputVector.x, inputVector.y, inputVector.z) * (myCamera.transform.rotation.y+ 0.1f) 0.;//Vector3.Cross(hitInfo.normal, new Vector3(input.x, 0f, input.y));
            //Debug.Log(input);
            forward = inputVector;//-Vector3.Cross(hitInfo.normal, myCamera.transform.right);   
            //forward = Vector3.Cross(hitInfo.normal, inputVector);
            //Debug.DrawRay(transform.position, Vector3.Cross(hitInfo.normal, inputVector), Color.green);
            //Debug.DrawRay(transform.position, forward * 5, Color.blue);
            //Debug.DrawRay(transform.position, hitInfo.normal * 5, Color.magenta);
        }
        else
        {
            if (!grounded)
            {
                forward = transform.forward;
                return;
            }
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.green);
            //Debug.DrawRay(hitInfo.point, -transform.right, Color.yellow);
            //Debug.DrawRay(transform.position,Vector3.Cross(hitInfo.normal, -transform.right), Color.red);
            forward = transform.forward;//Vector3.Cross(hitInfo.normal, -transform.right);
        }
    }

    /// <summary>
    /// Use a vector3 angle between the ground normal and the transform forward to determine the slope of the ground
    /// </summary>
    void CalculateGroundAngle()
    {
        if(!grounded)
        {
            groundAngle = 90;
            return;
        }
        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private bool IsBorderOK()
    {
        float forwardInput = Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y));
        Vector3 nextPos = transform.position + transform.forward * wallCheckLength;

        if (debug)
            Debug.DrawRay(nextPos, -transform.up, Color.red);

        //Checks to avoid falling into the abyss
        if (Physics.Raycast(nextPos, -transform.up, Mathf.Infinity, walkableLayerMask))
        {
            Vector3 epsilonRaycastStart = Vector3.up * heightPadding * 0.01f;

            //Check to avoid running into the walls
            Vector3 firstRay = (transform.forward + transform.forward + transform.right) * forwardInput;
            Vector3 secondRay = (transform.forward + transform.forward - transform.right) * forwardInput;

            if (debug)
            {
                Debug.DrawRay(transform.position - epsilonRaycastStart, firstRay.normalized * wallCheckLength, Color.red);
                Debug.DrawRay(transform.position - epsilonRaycastStart, secondRay.normalized * wallCheckLength, Color.red);
            }
            if (Physics.Raycast(transform.position - epsilonRaycastStart, firstRay, wallCheckLength,  ~walkableLayerMask) ||
                Physics.Raycast(transform.position - epsilonRaycastStart, secondRay, wallCheckLength, ~walkableLayerMask))
                return false;
            else
                return true;
        }
        else
            return false;
    }


    /// <summary>
    /// Use a raycast of length height to determine whether or not the player is grounded
    /// </summary>
    void CheckGround()
    {
        Vector3 epsilonRaycastStart = Vector3.up * heightPadding * 0.01f;
        if (Physics.Raycast(transform.position + epsilonRaycastStart, -Vector3.up, out hitInfo, height + heightPadding, walkableLayerMask))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < height)
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * height, 5 * Time.deltaTime);

            grounded = true;
            fallHitSet = false;
            fallVelocity = Vector3.zero;
        }
        else
            grounded = false;
    }

    /// <summary>
    /// If not grounded, the player should fall
    /// </summary>
    void ApplyGravity()
    {
        if (debug)
        {
            Debug.DrawLine(transform.position + transform.right, transform.position - transform.right, Color.cyan);
            Debug.DrawLine(transform.position + transform.up, transform.position - transform.up, Color.cyan);
        }

        if (!grounded)
        {
            fallVelocity += Physics.gravity;
            Vector3 newPosition = transform.position + fallVelocity * Time.deltaTime;

            if (debug)
                Debug.DrawRay(transform.position, -Vector3.up * 1000, Color.magenta);

            if (!fallHitSet)
            {
                if (Physics.Raycast(transform.position, -Vector3.up, out fallHitInfo, Mathf.Infinity, walkableLayerMask))
                {
                    fallHitSet = true;
                    newPosition.y = Mathf.Max(newPosition.y, fallHitInfo.point.y);
                }
            }
            else
            {
                newPosition.y = Mathf.Max(newPosition.y, fallHitInfo.point.y);
            }

            transform.position = newPosition;
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, hitInfo.point + Vector3.up * height, 10 * Time.deltaTime);
            transform.position =  hitInfo.point + Vector3.up * height;
        }
    }

    public void Attack()
    {
        myCharacterStatus.RequestAttack();
    }

    /// <summary>
    /// Draw debug lines of the grounded check
    /// ... the height
    /// ... and the height padding
    /// </summary>
    void DrawDebugLines()
    {
        if (!debug) return;

        //Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        //Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

}
