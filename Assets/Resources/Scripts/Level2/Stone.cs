using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        SacredFlames sacredFlames = collision.collider.GetComponent<SacredFlames>();
        Weapon weapon = collision.collider.GetComponent<Weapon>();
        if (sacredFlames && weapon && weapon.weaponHolder.AttackingStatus)
        {
            GetComponent<LordOfTheDeadsStatus>().PetrifiedStatus = true;
        }
    }
}
