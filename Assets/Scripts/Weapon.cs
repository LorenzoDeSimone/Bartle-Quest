using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Fighter hitEnemy = collision.collider.GetComponent<Fighter>();
        if (hitEnemy)
            hitEnemy.Hit(damage);

    }
}
