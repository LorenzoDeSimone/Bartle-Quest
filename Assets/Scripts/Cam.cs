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
    private float standardDistance;
    public LayerMask wallLayerMask;
    private bool lerping;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        standardDistance = distance;
    }

    void LateUpdate()
    {
        if (target)
        {
            float horizontal = Input.GetAxis("RightHorizontal");
            float vertical = Input.GetAxis("RightVertical");

            if (Mathf.Abs(horizontal) > 0.5f)
                x += (float)(horizontal * xSpeed * 0.02);

            if (zoom)
            {
                distance -= (float)(vertical);
            }
            else
            {
                if (Mathf.Abs(vertical) > 0.5f)
                    y += (float)(vertical * zoomSpeed * 0.02);
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

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
                if (Mathf.Abs(distance - standardDistance) > 0.1f)
                    distance = Mathf.Lerp(distance, standardDistance, Time.deltaTime * 2);
                else
                    distance = standardDistance;
                //}
            }
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    IEnumerator LerpBackToNormal(float initialDistance)
    {
        while(Mathf.Abs(distance - standardDistance) > 0.2f)
        {
            distance = Mathf.Lerp(initialDistance, standardDistance, Time.deltaTime * 2);
            yield return null;
        }
        distance = standardDistance;
        lerping = false;
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