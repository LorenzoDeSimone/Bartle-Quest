using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHead : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 playerOffset;
    [SerializeField] private float speed;
    [SerializeField] private bool isGilbert;

    private AnimationScript animationScript;

    private float acceleration = 0;

    void Start()
    {
        animationScript = GetComponent<AnimationScript>();
        TryToActivate();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 currentTarget = player.position + playerOffset;
        Vector3 direction = (currentTarget - transform.position);
        acceleration += speed * Time.deltaTime; 
        transform.position += direction * acceleration;
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), 0.1f);

        if (Vector3.Distance(transform.position, currentTarget) < 0.5f)
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * 0.25f);
            acceleration = 0f;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.1f);
        }

        Debug.DrawLine(transform.position, currentTarget, Color.red);
        //Debug.DrawRay(transform.position, direction * acceleration, Color.red);
        animationScript.startY = transform.position.y;
    }

    public bool TryToActivate()
    {
        if (isGilbert && PlayerChoices.Instance().HasHistorianGhost)
        {
            gameObject.SetActive(true);
            return true;
        }
        else if (!isGilbert && !PlayerChoices.Instance().HasHistorianGhost)
        {
            gameObject.SetActive(true);
            return true;
        }
        else
        {
            gameObject.SetActive(false);
            return false;
        }
    }
}
