using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 6, -8);
    public float xTilt = 10;

    private Vector3 destination = Vector3.zero;
    private MovingCharacter myMovingCharacter;

    float rotateVel = 0;

    public void SetCameraTarget(Transform t)
    {
        if (t != null)
        {
            target = t;
            if (target.GetComponent<MovingCharacter>())
                myMovingCharacter = target.GetComponent<MovingCharacter>();
            else
                Debug.LogError("Camera's target is not a character controller.");
        }
        else
            Debug.LogError("Camera needs a valid target.");
    }

    // Use this for initialization
    void Start()
    {
        SetCameraTarget(target);
    }

    void LateUpdate()
    {
        MoveToTarget();
        LookAtTarget();
    }

    void MoveToTarget()
    {
        destination = myMovingCharacter.TargetRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
    }
}
