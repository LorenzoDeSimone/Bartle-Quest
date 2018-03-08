using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterStatus : MonoBehaviour
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
        AnimatorStateInfo currentAnimatorState = myAnimator.GetCurrentAnimatorStateInfo(0);
        //if (AttackingStatus  && currentAnimatorState.fullPathHash != lastAnimatorState)

            //The character is not attacking and combo needs to start
        if (!AttackingStatus)
        {
            //Debug.Log(AttackStates[0] + "FIRST PRESS");
            //StartCoroutine(ReturnToIdle());
            myAnimator.SetTrigger("firstAttack");
            //currentAnimatorState = myAnimator.GetCurrentAnimatorStateInfo(0);
            //lastAnimatorState = currentAnimatorState.fullPathHash;
        }
        //If character is already attacking, input is registered to act when current animation is finished
        //If the button is pressed during last attack state, trigger combo should not be set
        /*else if(myAnimator.GetCurrentAnimatorStateInfo(0).IsName(AttackStates[AttackStates.Length-1]))
        {
            myAnimator.ResetTrigger("comboButtonPressed");
        }*/
        else if (AttackingStatus)
        {
            //lastAnimatorState = currentAnimatorState.fullPathHash;
            myAnimator.SetTrigger("comboButtonPressed");
            //currAnimationLenght = myAnimator.GetCurrentAnimatorStateInfo(0).length;
            //Debug.Log("COMBO PRESS");
        }
    }

    /*
    void Update()
    {
        if(AttackingStatus && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.25f)
            myAnimator.SetBool("comboButtonPressed", false);

        AnimatorStateInfo currentAnimatorState = myAnimator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimatorState.fullPathHash != lastAnimatorState)
            myAnimator.SetBool("comboButtonPressed", false);

    }*/

    private IEnumerator ReturnToIdle()
    {
        while(true)
        {
            //Debug.Log(myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (AttackingStatus && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                myAnimator.SetTrigger("returnToIdle");
            yield return null;
        }
    }

    public void DisableTransitionsToFalse()
    {
        myAnimator.SetBool("disableTransitions", false);
        Debug.Log("q");
    }
}