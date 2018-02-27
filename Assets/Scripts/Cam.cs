using System.Collections;
using UnityEngine;


public class Cam : MonoBehaviour
{
    public Transform target;
    private Transform defaultTarget;

    private TargetManager defaultTargetManager;

    public float distance = 10.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float zoomSpeed = 120.0f;
    private float x = 0.0f;
    private float y = .0f;
    public float lockDistance = 8f;
    private float standardDistance;
    public LayerMask wallLayerMask;
    private bool lerping;
    [SerializeField] private float lockCamMinDistance = 3f;
    [SerializeField] private float maxTargetDistance = 10f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        defaultTarget = target;
        defaultTargetManager = target.GetComponent<TargetManager>();
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

    bool IsRightStickPressed()
    {
        return true;
    }

    void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position;
        transform.rotation = rotation;

        //Check walls
        RaycastHit hitInfo;
        Vector3 cameraFocusPoint = GetCameraFocusPoint();
        float distanceBetween = Vector3.Distance(defaultTarget.position, target.position);//0 if target is the player
        distance = distanceBetween + lockCamMinDistance;
        Vector3 idealCameraPos = cameraFocusPoint + rotation * new Vector3(0.0f, 0.0f, -distance);

        Debug.DrawRay(defaultTarget.position, idealCameraPos - cameraFocusPoint, Color.green);

        if (Physics.Raycast(defaultTarget.position, (idealCameraPos - cameraFocusPoint).normalized,
            out hitInfo, Vector3.Distance(defaultTarget.position, idealCameraPos), wallLayerMask))
        {

            if (Physics.Raycast(cameraFocusPoint, (idealCameraPos - cameraFocusPoint).normalized,
                out hitInfo, Vector3.Distance(defaultTarget.position, idealCameraPos), wallLayerMask))
            //Debug.DrawRay(target.position, position - target.position, Color.red); 
            {
                //if (Vector3.Distance(transform.position, hitInfo.point) < 0.1f)
                    position = hitInfo.point;
                //else
                //    position = Vector3.Lerp(transform.position, hitInfo.point, Time.deltaTime * 10);
            }
            else
            {
                //if (Vector3.Distance(transform.position, idealCameraPos) < 0.1f)
                    position = idealCameraPos;
                //else
                //    position = Vector3.Lerp(transform.position, idealCameraPos, Time.deltaTime * 10);
            }

            distance = Vector3.Distance(cameraFocusPoint, hitInfo.point);
            Debug.DrawLine(hitInfo.point + transform.right, hitInfo.point - transform.right, Color.cyan);
            Debug.DrawLine(hitInfo.point + transform.up, hitInfo.point - transform.up, Color.cyan);
        }
        else// if (!target.Equals(defaultTarget))
            position = idealCameraPos;
        //else
        //    position = Vector3.Lerp(transform.position, idealCameraPos, Time.deltaTime * 10);

        transform.position = position;
    }

    Vector3 GetCameraFocusPoint()
    {
        if (!defaultTarget.Equals(target))
        {
            Vector3 middlePoint = new Vector3((defaultTarget.position.x + target.position.x) * 0.5f, (defaultTarget.position.y + target.position.y) * 0.5f,
                                                                                                    (defaultTarget.position.z + target.position.z) * 0.5f);
            return middlePoint;
        }
        else
            return defaultTarget.position;
    }

    void UpdateTarget()
    {
        if (IsTargetPressed())
        {
            Transform lockTarget = defaultTargetManager.GetNearestTarget();
            if (lockTarget != null)
            {
                //distance += Vector3.Distance(defaultTarget.position, lockTarget.position);
                target = lockTarget;
            }
        }
        else if (IsTargetReleased())
        {
            target = defaultTarget;
            //distance = standardDistance;
        }

    }

    bool IsTargetPressed()
    {
        return Input.GetButtonDown("TargetLock");
        //!Physics.Raycast(transform.position, (transform.position - GetCameraFocusPoint()).normalized, Mathf.Infinity, wallLayerMask);
    }

    bool IsTargetReleased()
    {
        if (target.Equals(defaultTarget))
            return true;
        else
            return (Input.GetButtonUp("TargetLock") || Vector3.Distance(target.position, defaultTarget.position) > maxTargetDistance);// || 
                                                                                                                                      //Physics.Raycast(transform.position, (transform.position - GetCameraFocusPoint()).normalized, Mathf.Infinity, wallLayerMask));
    }

    void LateUpdate()
    {
        UpdateTarget();
        GetCameraInput();
        UpdatePosition();
        /*if (target)
        {

            if (IsTriggerDown() && !IsTargetInsideView())
            {
                //distance = Vector3.Distance(target.position, transform.position);
                //distance = Mathf.Lerp(distance, distance + 1f, Time.deltaTime * 50);
                distance += 0.1f;
                Debug.Log(target.gameObject.name);
            }
            else if(IsTargetInsideView() && !IsTriggerDown())
            {          
                distance = standardDistance;
                //Debug.Log("A");
                Debug.Log(target.gameObject.name);
            }


            Vector3 position = target.position + rotation * new Vector3(0.0f, 0.0f, -distance);


            /*
            //Check walls
            RaycastHit hitInfo;
            Debug.DrawRay(position, target.position - position, Color.green);
            if (Physics.Raycast(target.position, (position - target.position).normalized, out hitInfo, Vector3.Distance(target.position, position), wallLayerMask))
            {
                //Debug.DrawRay(target.position, position - target.position, Color.red);   
                position = hitInfo.point;
                distance = Vector3.Distance(target.position, hitInfo.point);
                Debug.DrawLine(hitInfo.point + transform.right, hitInfo.point - transform.right, Color.cyan);
                Debug.DrawLine(hitInfo.point + transform.up, hitInfo.point - transform.up, Color.cyan);
            }
            else
            {
                //if (!lerping)
                //{
                //    lerping = true;
                //StartCoroutine(LerpBackToNormal(distance));
                Vector3 screenPoint = GetComponent<Camera>().WorldToViewportPoint(defaultTarget.position);
                /*if (!(screenPoint.z > -0.2f && screenPoint.x > -0.2f && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1))
                {
                    distance = Mathf.Lerp(distance, distance + 1f, Time.deltaTime * 10);
                    Debug.Log("A");
                }
                else
                {
                //float triggerPressed = Input.GetAxisRaw("TargetLock");

                //if (triggerPressed < 0.9f)
                //{

                if (Mathf.Abs(distance - standardDistance) > 0.1f)
                {
                    Debug.Log("B");
                    distance = Mathf.Lerp(distance, standardDistance, Time.deltaTime * 2);
                }
                else
                {
                    Debug.Log("C");
                    distance = standardDistance;
                }
                
            }
          
            transform.rotation = rotation;
            transform.position = position;
        }*/
    }

    bool IsTriggerDown()
    {
        return Input.GetButton("TargetLock");
    }


    bool IsTriggerToggled()
    {
        return Input.GetButtonDown("TargetLock");
    }

    IEnumerator LerpBackToNormal(float initialDistance)
    {
        while (Mathf.Abs(distance - standardDistance) > 0.01f)
        {
            distance = Mathf.Lerp(initialDistance, standardDistance, Time.deltaTime * 10);
            Debug.Log("lerping");
            yield return null;
        }
        distance = standardDistance;
        lerping = false;
        Debug.Log("LerpDone");
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