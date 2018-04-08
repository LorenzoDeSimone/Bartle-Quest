using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadsStatus : EnemyStatus
{
    [SerializeField] public Transform[] teleportPoints;

    public bool PetrifiedStatus
    {
        get
        {
            return myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Petrified") || myAnimator.GetAnimatorTransitionInfo(0).IsName("AnyState -> Petrified");
        }
        set
        {
            if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Petrified"))
                myAnimator.SetTrigger("petrified");
        }
    }
}
