using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. 8-directional movement
/// 2. stop and face current direction when input is absent
/// </summary>

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float inputEpsilon = 0.1f;
    [SerializeField] private float runningThreshold = 0.5f;
    [SerializeField] private float velocity = 6f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float heightPadding = 0.5f;
    [SerializeField] private float wallCheckLength = 1f;
    [SerializeField] private float maxGroundAngle = 150;
    [SerializeField] private LayerMask walkableLayerMask;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private bool debug;
    [SerializeField] private DialogueManager dialogueManager;

    private bool fallHitSet;
    private float angle;
    private float groundAngle;
    private Vector2 input;
    private Vector3 forward;
    private Vector3 fallVelocity;
    private Quaternion targetRotation;
    private Transform myCamera;
    private RaycastHit hitInfo, fallHitInfo;
    private CharacterStatus myPlayerStatus;
    private PlayerCamera myCameraScript;

    private int previousFrameVerticalRaw = 0;

    void Start()
    {
        myCamera = Camera.main.transform;
        walkableLayerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Stairs");
        wallLayerMask = LayerMask.GetMask("Wall") | LayerMask.GetMask("Obstacle") | LayerMask.GetMask("NPC") | LayerMask.GetMask("Ally");
        myPlayerStatus = GetComponent<CharacterStatus>();
        myCameraScript = myCamera.GetComponent<PlayerCamera>();
    }

    void Update()
    {
        GetInput();

        if (IsGamePaused())
        {
            //Dialogue controls
            if (DialogueManager.IsDialogueOn)
                GameDialogueUpdate();
            else //Actual pause
                GamePausedUpdate();
        }
        else
        {
            GameUnPausedUpdate();
        }
    }

    private void GameDialogueUpdate()
    {
        myPlayerStatus.MovingStatus = CharacterStatus.movingIdleValue;
        int absVerticalAxisRaw = (int) Mathf.Abs(Input.GetAxisRaw("Vertical"));
        if (absVerticalAxisRaw > previousFrameVerticalRaw)
            dialogueManager.HightLightChoice((int)Mathf.Sign(input.y));

        if (Input.GetButtonDown("A"))
            dialogueManager.DialogueChoiceConfirmed();

        previousFrameVerticalRaw = absVerticalAxisRaw;
    }

    private void GamePausedUpdate()
    {
        myPlayerStatus.MovingStatus = CharacterStatus.movingIdleValue;
        if (Input.GetButtonDown("Y"))
            PauseAndDeathManager.Instance().ReloadScene();
    }

    private void InteractionInput()
    {
        //Debug.Log(myCameraScript.interactionManager.GetNearestTarget());

        Transform currentInteractionTarget = myCameraScript.interactionManager.GetNearestTarget();
        if (currentInteractionTarget != null)
        {
            string[] buttonNames = { "A", "B", "Y" };

            foreach (string buttonName in buttonNames)
            {
                if (Input.GetButtonDown(buttonName))
                    currentInteractionTarget.GetComponent<Target>().Interact(buttonName);
            }
        }
    }

    private void GameUnPausedUpdate()
    {
        if (myPlayerStatus.DeathStatus)
            return;

        if (Input.GetButtonDown("X"))
            Attack();

        InteractionInput();
        UpdateShieldStatus();

        CalculateDirection();
        CalculateForward();
        CalculateGroundAngle();
        CheckGround();
        ApplyGravity();
        DrawDebugLines();
        Rotate();
        Move();
    }

    private bool IsGamePaused()
    {
        //Debug.Log("actual " +Time.timeScale);
        //Debug.Log("default " + defaultTimeScale);
        return Time.timeScale < 1f;
    }

    /// <summary>
    /// Input based on Horizontal (a,d,<,>) and Vertical (w,s,^,v) keys
    /// </summary>
    void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    void UpdateShieldStatus()
    {
        bool cameraTargetIsEnemy = false;

        if (myCameraScript.CurrentTarget != null)
        {
            Target target = myCameraScript.CurrentTarget.GetComponent<Target>();
            cameraTargetIsEnemy = target != null && target.IsEnemy;
        }

        myPlayerStatus.ShieldUpStatus =  Input.GetAxis("LT") > 0.1f      && 
                                         myPlayerStatus.GroundedStatus   &&
                                         !myPlayerStatus.AttackingStatus &&
                                         (cameraTargetIsEnemy || myCameraScript.CurrentTarget.Equals(transform));
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

            if (myPlayerStatus.ShieldUpStatus)
                return;

            if (myPlayerStatus.AttackingStatus)
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
            myPlayerStatus.MovingStatus = CharacterStatus.movingIdleValue;
        }
        else
        {
            myPlayerStatus.MovingStatus = CharacterStatus.movingRunValue;

            transform.position += forward * velocity * sign * Time.deltaTime;
        }
    }

    void ClampInput(float threshold)
    {
        if(Mathf.Abs(input.x) > threshold)
        {
            if (input.x < 0f)
                input.x = -threshold;
            else
                input.x = threshold;
        }

        if (Mathf.Abs(input.y) > threshold)
        {
            if (input.y < 0f)
                input.y = -threshold;
            else
                input.y = threshold;
        }
    }

    /// <summary>
    /// This player only moves along its own forward axis
    /// </summary>
    void Move()
    {

        //Debug.Log(Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)));

        
        if (groundAngle > maxGroundAngle || myPlayerStatus.AttackingStatus || !IsBorderOK())
        {
            myPlayerStatus.MovingStatus = CharacterStatus.movingIdleValue;
            return;
        }

        if (myPlayerStatus.ShieldUpStatus)
            ClampInput(runningThreshold);

        if (Mathf.Abs(input.x) < inputEpsilon && Mathf.Abs(input.y) < inputEpsilon)
        {
            myPlayerStatus.MovingStatus = CharacterStatus.movingIdleValue;
        }
        else if (Mathf.Abs(input.x) <= runningThreshold && Mathf.Abs(input.y) <= runningThreshold)
        {
            myPlayerStatus.MovingStatus = CharacterStatus.movingWalkValue;
            transform.position += forward * velocity * Mathf.Max(Mathf.Abs(input.x),Mathf.Abs(input.y)) * Time.deltaTime;
        }
        else
        {
            myPlayerStatus.MovingStatus = CharacterStatus.movingRunValue;
            transform.position += forward * velocity * Mathf.Max(Mathf.Abs(input.x), Mathf.Abs(input.y)) * Time.deltaTime;
        }
    }

    private Vector3 GetForwardFromInputVector()
    {
        Vector3 inputVector = (myCamera.transform.TransformDirection(input)).normalized;

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

        if(myPlayerStatus.GroundedStatus)
        {

            Vector3 cameraForward = myCamera.forward;
            Vector3 cameraRight = myCamera.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            forward = (cameraForward * input.y + cameraRight * input.x).normalized;
            //Debug.DrawRay(transform.position, forward * 2, Color.red);
        }
        /*else
        {
            if (!myPlayerStatus.GroundedStatus)
            {
                forward = transform.forward;
                return;
            }
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.green);
            //Debug.DrawRay(hitInfo.point, -transform.right, Color.yellow);
            //Debug.DrawRay(transform.position,Vector3.Cross(hitInfo.normal, -transform.right), Color.red);
            forward = transform.forward;//Vector3.Cross(hitInfo.normal, -transform.right);
        }*/
    }

    /// <summary>
    /// Use a vector3 angle between the ground normal and the transform forward to determine the slope of the ground
    /// </summary>
    void CalculateGroundAngle()
    {
        if(!myPlayerStatus.GroundedStatus)
        {
            groundAngle = 90;
            return;
        }
        groundAngle = Vector3.Angle(hitInfo.normal, transform.forward);
    }

    private bool IsBorderOK()
    {
        Vector3 nextPos = transform.position + forward * wallCheckLength;

        if (debug)
            Debug.DrawRay(nextPos, -transform.up, Color.red);

        //Checks to avoid falling into the abyss
        if (Physics.Raycast(nextPos, -transform.up, Mathf.Infinity, walkableLayerMask))
        {
            //Check to avoid running into the walls
            if (debug)
                Debug.DrawRay(transform.position, forward * wallCheckLength, Color.red);

            if (Physics.Raycast(transform.position, forward, wallCheckLength, wallLayerMask))
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

            myPlayerStatus.GroundedStatus = true;
            fallHitSet = false;
            fallVelocity = Vector3.zero;
        }
        else
            myPlayerStatus.GroundedStatus = false;
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

        if (!myPlayerStatus.GroundedStatus)
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
        myPlayerStatus.RequestAttack();
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
