using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public CharacterStatus weaponHolder;

    [SerializeField]
    public int damage = 1;

    [SerializeField]
    private GameObject WeaponHitEffect;

    private Animator myAnimator;
    private int lastAnimatorState = -1;

    // Use this for initialization
    void Start ()
    {
        if (weaponHolder != null)
        {
            myAnimator = weaponHolder.GetComponent<Animator>();
            if (myAnimator == null)
                Debug.LogError("The weapon holder as no animator.");
        }
	}

    void OnCollisionStay(Collision collision)
    {
        if (!weaponHolder)
            return;
        //Hits only if it is the first collision in the current animator state

        //Debug.Log(weaponHolder.gameObject.name +" Hits" + collision.collider.name);

        //if (weaponHolder.gameObject.name.Equals("Guard"))
        //    Debug.Log(currentAnimatorState.fullPathHash != lastAnimatorState);

        if (weaponHolder.CanWeaponHit(collision.gameObject) && weaponHolder.AttackingStatus && !weaponHolder.gameObject.Equals(collision.collider.gameObject)) 
            //currentAnimatorState.fullPathHash != lastAnimatorState)
        {
            Shield hitShield = collision.collider.GetComponent<Shield>();
            if (hitShield && hitShield.GetShieldHolder()!=null && hitShield.GetShieldHolder().ShieldUpStatus)
            {
                if (weaponHolder.CanWeaponHit(hitShield.GetShieldHolder().gameObject))
                {
                    hitShield.ActivateBlockEffect();
                    weaponHolder.AttackBlocked();
                }

                weaponHolder.AddHitEnemy(hitShield.GetShieldHolder().gameObject, true);
            }
            else
            {
                /*if (!weaponHolder.gameObject.Equals("Guard"))
                {
                    Debug.Log(weaponHolder.gameObject.name + " Hits" + collision.collider.name);
                }*/
                //lastAnimatorState = currentAnimatorState.fullPathHash;
                Hittable hitTarget = collision.collider.GetComponent<Hittable>();
                //Check to avoid "friendly fire"
                if (hitTarget && !hitTarget.gameObject.layer.Equals(weaponHolder.gameObject.layer))
                {
                    weaponHolder.AddHitEnemy(collision.gameObject);
                    //Debug.Log(weaponHolder.gameObject.name + " hits " + collision.gameObject);
                    hitTarget.UpdateHealth(-damage);
                    if (WeaponHitEffect != null)
                    {
                        GameObject hitEffect = Instantiate(WeaponHitEffect, collision.contacts[0].point, collision.collider.transform.rotation);
                        hitEffect.transform.parent = collision.gameObject.transform;
                    }
                }
            }
        }
    }
}
