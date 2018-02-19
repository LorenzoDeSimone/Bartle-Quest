using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1. Follow on players' X/Z plane
/// 2. Smooth rotations around the player in 45 degree increments
/// </summary>
public class CameraController8D : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float moveSpeed = 5;
    public float turnSpeed = 10;

    Quaternion targetRotation;
    Vector3 targetPos;

    bool smoothRotating = false;

    void LateUpdate()
    {
        LookAtTarget();
        MoveCamera();
        //StartCoroutine("RotateAroundTarget", 45);
        //}
        //if (Input.GetAxis("RightVertical") > 0.0f)// && !smoothRotating)
        //{
        //    Debug.Log(Input.GetAxis("RightVertical"));
        //StartCoroutine("RotateAroundTarget", -45);
        //}
        }

    void MoveCamera()
    {
        targetPos = target.position + offsetPos;

        Vector3 directionToTarget = (target.position - targetPos).normalized;
        Vector3 movementVector = Vector3.zero;

        float horizontal = Input.GetAxis("RightHorizontal");
        float vertical = Input.GetAxis("RightVertical");

        Debug.DrawRay(targetPos, directionToTarget, Color.red);

        int horiSign = 1, vertSign = 1;

        if (Mathf.Abs(horizontal) > 0.4f)
        {
            if (horizontal < 0f)
                horiSign = -1;

            movementVector -= (Vector3.Cross(directionToTarget, -transform.up) * horiSign);
            Debug.DrawRay(targetPos, movementVector , Color.green);
        }

        if (Mathf.Abs(vertical) > 0.4f)
        {
            if (vertical < 0f)
                vertSign = -1;

            movementVector += (Vector3.Cross(directionToTarget, transform.right) * horiSign);
            Debug.DrawRay(targetPos, movementVector, Color.green);
        }

        offsetPos += movementVector * turnSpeed * Time.smoothDeltaTime;
        
        //transform.position = target.position + offsetPos;

        transform.position = Vector3.Lerp(transform.position, target.position + offsetPos, moveSpeed * Time.smoothDeltaTime);

    }

    /// <summary>
    /// Use the look vector (target - current) to aim the camera toward the player 
    /// </summary>
    void LookAtTarget()
    {
        //targetRotation = 
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
   
}
