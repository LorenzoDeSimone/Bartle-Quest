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

    private Animator myAnimator;
    private int lastAnimatorState = -1;

    // Use this for initialization
    void Start ()
    {
        if (weaponHolder == null)
            Debug.LogError("This weapon has no holder.");
        else
        {
            myAnimator = weaponHolder.GetComponent<Animator>();
            if (myAnimator == null)
                Debug.LogError("The weapon holder as no animator.");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //If attacking is finished, last animator state reset
        if(!weaponHolder.AttackingStatus)
            lastAnimatorState = -1;
    }

    void OnCollisionEnter(Collision collision)
    {

        //Hits only if it is the first collision in the current animator state
        AnimatorStateInfo currentAnimatorState = myAnimator.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(currentAnimatorState.fullPathHash);

        if (weaponHolder.AttackingStatus && currentAnimatorState.fullPathHash != lastAnimatorState)
        {
            lastAnimatorState = currentAnimatorState.fullPathHash;
            Debug.Log("Hit");
            Hittable hitTarget = collision.collider.GetComponent<Hittable>();
            if (hitTarget)
                hitTarget.Hit(damage);
        }
    }
}
