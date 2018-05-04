using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    [SerializeField] private float secondsToFall = 2f, floatRate = 1f, floatSpeed = 1;
    [SerializeField] private float yDistanceToDisappear;
    private Vector3 fallVelocity = Vector3.zero;
    private static float distanceBeforeDisappear;
    private float finalY, initialY;

    void Start()
    {
        initialY = transform.position.y;
        finalY = transform.position.y + yDistanceToDisappear;
        //StartCoroutine(ApplyGravity());

    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
            Fall();
    }

    public void Fall()
    {
        StartCoroutine(ApplyGravity());
    }

    private IEnumerator ApplyGravity()
    {
        float elapsedTime = 0f;

        while(elapsedTime < secondsToFall)
        {
            elapsedTime += Time.deltaTime;
            transform.position = new Vector3(transform.position.x,
                                                 initialY + floatRate * Mathf.Sin(floatSpeed * elapsedTime),
                                                 transform.position.z);
            yield return null;
        }
        
        while (transform.position.y > finalY)
        { 
            fallVelocity += Physics.gravity;
            transform.position += fallVelocity * Time.deltaTime;
            yield return null;
        }
 
        Destroy(gameObject);
    }
}
