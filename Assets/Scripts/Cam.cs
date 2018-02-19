using UnityEngine;
using System.Collections;


public class Cam : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float zoomSpeed = 120.0f;
    private float x = 0.0f;
    private float y = .0f;
    public bool zoom;


    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void LateUpdate()
    {
        if (target)
        {
            float horizontal = Input.GetAxis("RightHorizontal");
            float vertical = Input.GetAxis("RightVertical");

            if (Mathf.Abs(horizontal) > 0.4f)
                x += (float)(horizontal * xSpeed * 0.02);

            if (zoom)
            {
                distance -= (float)(vertical);
            }
            else
            {
                if (Mathf.Abs(vertical) > 0.4f)
                    y += (float)(vertical * zoomSpeed * 0.02);
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
    private int ClampAngle(float angle, float min, float max)
    {
        Debug.Log(angle);

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