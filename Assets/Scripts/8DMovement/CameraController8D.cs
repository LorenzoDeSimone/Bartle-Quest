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
    public float smoothSpeed = 0.5f;

    Quaternion targetRotation;
    Vector3 targetPos;
    bool smoothRotating = false;

    void LateUpdate()
    {
        MoveWithTarget();
        LookAtTarget();

        //if (Input.GetAxis("RightHorizontal") > 0.0f)// && !smoothRotating)
        //{
            Debug.Log(Input.GetAxis("RightHorizontal"));
            //Debug.Log(Input.GetAxisRaw("RightHorizontal"));

        //StartCoroutine("RotateAroundTarget", 45);
        //}
        //if (Input.GetAxis("RightVertical") > 0.0f)// && !smoothRotating)
        //{
        //    Debug.Log(Input.GetAxis("RightVertical"));
        //StartCoroutine("RotateAroundTarget", -45);
        //}
    }

    /// <summary>
    /// Move the camera to the player position + current camera offset
    /// Offset is modifiet by the RotateAroundTarget coroutine
    /// </summary>
    void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Use the look vector (target - current) to aim the camera toward the player 
    /// </summary>
    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This coroutime can only have one instance running at a time
    /// Determined by 'smoothRotating'
    /// </summary>
    IEnumerator RotateAroundTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        smoothRotating = true;
        while(dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
            yield return null;
        }

        smoothRotating = false;
        offsetPos = targetOffsetPos;
    }
    

}
