using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus weaponHolder;

    [SerializeField]
    private int damage = 1;

    // Use this for initialization
    void Start ()
    {
        if (weaponHolder == null)
            Debug.LogError("This weapon has no holder.");
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (weaponHolder.AttackingStatus)
        {
            Debug.Log("Hit");
            Hittable hitTarget = collision.collider.GetComponent<Hittable>();
            if (hitTarget)
                hitTarget.Hit(damage);
        }
    }
}
