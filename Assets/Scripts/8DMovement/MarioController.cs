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

    CharacterStatus myCharacterStatus;

    public float velocity = 5;
    public float turnSpeed = 10;

    public float height = 0.5f;
    public float heightPadding = 0.05f;
    public LayerMask ground;
    public float maxGroundAngle = 120;
    public bool debug;

    Vector2 input;
    float angle;
    float groundAngle;

    Quaternion targetRotation;
    Transform cam;

    Vector3 forward;
    RaycastHit hitInfo, fallHitInfo;
    bool grounded;
    bool fallHitSet;

    Vector3 fallVelocity;

    private static readonly int idleValue = 0, walkingValue = 1, runningValue = 2;

    void Start()
    {
        cam = Camera.main.transform;
        ground = LayerMask.GetMask("Ground");
        myCharacterStatus = GetComponent<CharacterStatus>();
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
        angle += cam.eulerAngles.y;
    }

    /// <summary>
    /// Rotate toward the calculated angle
    /// </summary>
    void Rotate()
    {
        if (Mathf.Abs(input.x) < inputEpsilon && Mathf.Abs(input.y) < inputEpsilon)
            return;

        if (myCharacterStatus.AttackingStatus)
            return;

        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This player only moves along its own forward axis
    /// </summary>
    void Move()
    {
        if (groundAngle > maxGroundAngle || myCharacterStatus.AttackingStatus)
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

    /// <summary>
    /// If the player is not grounded, forward will be equal to transform forward
    /// Use a cross product to determine the new forward vector
    /// </summary>
    void CalculateForward()
    {
        if (!grounded)
        {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(hitInfo.normal, -transform.right);

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

    /// <summary>
    /// Use a raycast of length height to determine whether or not the player is grounded
    /// </summary>
    void CheckGround()
    {
        if (Physics.Raycast(transform.position + Vector3.up * heightPadding * 0.01f, -Vector3.up, out hitInfo, height + heightPadding, ground))
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

            newPosition.y = Mathf.Max(newPosition.y, fallHitInfo.point.y);

            if (!fallHitSet)
            {
                if (Physics.Raycast(transform.position, -Vector3.up, out fallHitInfo, Mathf.Infinity, ground))
                    fallHitSet = true;

                newPosition.y = Mathf.Max(newPosition.y, fallHitInfo.point.y);
            }
            else
            {
                newPosition.y = Mathf.Max(newPosition.y, fallHitInfo.point.y);
            }
            transform.position = newPosition;// Physics.gravity * Time.deltaTime;
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

        Debug.DrawLine(transform.position, transform.position + forward * height * 2, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * height, Color.green);
    }

}
