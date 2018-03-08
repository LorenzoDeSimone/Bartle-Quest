using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private string[] AttackStates;
    
    private Animator myAnimator;

    private bool shieldUpThisFrame;
    private float currAnimationLenght;
    private int lastAnimatorState;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
            Debug.LogError("No animator!");
    }

    public bool ShieldUpStatus
    {
        get { return myAnimator.GetBool("isShieldUp"); }
        set { myAnimator.SetBool("isShieldUp", value); }
    }

    public int MovingStatus
    {
        get { return myAnimator.GetInteger("isMoving"); }
        set { myAnimator.SetInteger("isMoving", value); }
    }

    public bool GroundedStatus
    {
        get { return myAnimator.GetBool("isGrounded"); }
        set
        {
            if (GroundedStatus != value && !value)
                myAnimator.SetTrigger("isFalling");

            myAnimator.SetBool("isGrounded", value);
        }
    }

    public bool AttackingStatus
    {
        //Checks if the character is in one of the states labelled as attackStates in the animator
        get
        {
            if (myAnimator.GetAnimatorTransitionInfo(0).IsName("AnyState -> " + AttackStates[0]))
                return true;

            foreach (string attackState in AttackStates)
            {
                if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(attackState))
                    return true;
            }
            return false;
        }
    }

    public bool DeathStatus
    {
        get { return myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"); }
        set { if(!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                myAnimator.SetTrigger("isDead"); }
    }

    public void RequestAttack()
    {
        //The character is not attacking and combo needs to start
        if (!AttackingStatus)
            myAnimator.SetTrigger("firstAttack");
        else
            myAnimator.SetTrigger("comboButtonPressed");
    }

    public void DisableTransitionsToFalse()
    {
        myAnimator.SetBool("disableTransitions", false);
    }
}