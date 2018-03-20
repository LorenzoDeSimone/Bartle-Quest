using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchColumn : MonoBehaviour
{
    public static float rotationTime = 2f;

    private static float rotationEpsilon = 0.01f;
    public bool clockwise;
    public bool canRotate = true;

    public void Rotate()
    {
        Quaternion startRotation, endRotation;

        if (!canRotate)
            return;

        canRotate = false;

        if (clockwise)
        {
            startRotation = transform.rotation;
            endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 90, transform.rotation.eulerAngles.z);
        }
        else
        {
            startRotation = transform.rotation;
            endRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);
        }

        StartCoroutine(Rotation(startRotation, endRotation));
    }

    private IEnumerator Rotation(Quaternion startRotation, Quaternion endRotation)
    {
        float elapsedTime = 0;

        while (elapsedTime < rotationTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationTime);
            yield return null;
        }

        canRotate = true;
        transform.rotation = endRotation;
    }
}