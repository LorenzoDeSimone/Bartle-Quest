using System.Collections;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float xSpeed = 250.0f;
    [SerializeField] private float ySpeed = 120.0f;
    [SerializeField] private float yMinLimit = -20f;
    [SerializeField] private float yMaxLimit = 80f;
    [SerializeField] private float zoomSpeed = 120.0f;
    [SerializeField] private float x = 0.0f;
    [SerializeField] private float y = 0.0f;
    [SerializeField] private float lockDistance = 8f;
    [SerializeField] private float lockCamMinDistance = 3f;
    [SerializeField] private float maxTargetDistance = 10f;
    [SerializeField] private LayerMask wallLayerMask;

    private Transform player;
    private TargetManager defaultTargetManager;
    private float standardDistance;
    private CharacterStatus myPlayerStatus;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        player = target;
        defaultTargetManager = player.GetComponent<TargetManager>();
        myPlayerStatus = player.GetComponent<CharacterStatus>();
        x = angles.y;
        y = angles.x;
        standardDistance = distance;
    }

    private void GetCameraInput()
    {
        float horizontal = Input.GetAxis("RightHorizontal");
        float vertical = Input.GetAxis("RightVertical");

        if (Mathf.Abs(horizontal) > 0.5f)
            x += (float)(horizontal * xSpeed * 0.02);

        if (Mathf.Abs(vertical) > 0.5f)
            y += (float)(vertical * zoomSpeed * 0.02);

        y = ClampAngle(y, yMinLimit, yMaxLimit);
    }

    void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position;
        transform.rotation = rotation;

        RaycastHit hitInfo;
        Vector3 cameraFocusPoint = GetCameraFocusPoint();
        float distanceBetween = Vector3.Distance(player.position, target.position);//0 if target is the player
        Vector3 idealCameraPos = cameraFocusPoint + rotation * new Vector3(0.0f, 0.0f, -(distanceBetween * 0.5f + lockCamMinDistance));

        //Debug.DrawRay(defaultTarget.position, idealCameraPos - cameraFocusPoint, Color.green);

        //Check walls
        if (Physics.Raycast(player.position, (idealCameraPos - cameraFocusPoint).normalized,
            out hitInfo, Vector3.Distance(player.position, idealCameraPos), wallLayerMask))
        {

            if (Physics.Raycast(cameraFocusPoint, (idealCameraPos - cameraFocusPoint).normalized,
                out hitInfo, Vector3.Distance(player.position, idealCameraPos), wallLayerMask))
            {
                distance = Vector3.Distance(cameraFocusPoint, hitInfo.point);
                position = hitInfo.point;//We want the point hit by the cameraFocus, not the character
            }
            else
            {
                distance = Mathf.Lerp(distance, distanceBetween * 0.5f + lockCamMinDistance, Time.deltaTime * 10);
                idealCameraPos = cameraFocusPoint + rotation * new Vector3(0.0f, 0.0f, -distance);
                position = idealCameraPos;
            }
            //distance = Vector3.Distance(cameraFocusPoint, hitInfo.point);
            //Debug.DrawLine(hitInfo.point + transform.right, hitInfo.point - transform.right, Color.cyan);
            //Debug.DrawLine(hitInfo.point + transform.up, hitInfo.point - transform.up, Color.cyan);
        }
        else
        {
            distance = Mathf.Lerp(distance, distanceBetween * 0.5f + lockCamMinDistance, Time.deltaTime * 10);
            idealCameraPos = cameraFocusPoint + rotation * new Vector3(0.0f, 0.0f, -distance);
            position = idealCameraPos;
        }
        transform.position = position;
    }

    Vector3 GetCameraFocusPoint()
    {
        if (!player.Equals(target))
        {
            Vector3 middlePoint = new Vector3((player.position.x + target.position.x) * 0.5f, (player.position.y + target.position.y) * 0.5f,
                                                                                                    (player.position.z + target.position.z) * 0.5f);
            return middlePoint;
        }
        else
            return player.position;
    }

    void UpdateTarget()
    {
        if (IsTargetPressed())
        {
            Transform lockTarget = defaultTargetManager.GetNearestTarget();
            if (lockTarget != null)
                target = lockTarget;
        }
        else if (IsTargetReleased())
            target = player;

    }

    public Transform GetTarget()
    {
        return target;
    }

    public Transform GetDefaultTarget()
    {
        return player;
    }

    public bool IsInLockTargetStatus()
    {
        return !player.Equals(target);
    }

    public bool IsTargetPressed()
    {
        return Input.GetButtonDown("TargetLock");
    }

    public bool IsTargetReleased()
    {
        if (target.Equals(player))
            return true;
        else
            return (Input.GetButtonUp("TargetLock") || Vector3.Distance(target.position, player.position) > maxTargetDistance);
    }

    void LateUpdate()
    {
        UpdateTarget();
        GetCameraInput();
        UpdatePosition();
    }

    bool IsTriggerDown()
    {
        return Input.GetButton("TargetLock");
    }


    bool IsTriggerToggled()
    {
        return Input.GetButtonDown("TargetLock");
    }

    private int ClampAngle(float angle, float min, float max)
    {
        //Debug.Log(angle);

        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp((int)(angle), (int)(min), (int)(max));
    }

}