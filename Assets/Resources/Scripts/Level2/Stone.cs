using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<SacredFlames>())
        {
            GetComponent<GuardStatus>().DeathStatus = true;
        }
    }
}
