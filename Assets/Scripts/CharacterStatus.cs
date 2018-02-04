using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterStatus : MonoBehaviour
{

    [SerializeField]
    private string[] AttackStates;
    private int comboCounter = -1;

    private bool nextAttackButtonPressed = false;

    private Animator myAnimator;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public bool RunningStatus
    {
        get { return myAnimator.GetBool("isRunning"); }
        set { myAnimator.SetBool("isRunning", value); }
    }

    public bool AttackingStatus
    {
        //Checks if the character is in one of the states labelled as attackStates in the animator
        get
        {
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
        get { return myAnimator.GetBool("isDead"); }
        set { myAnimator.SetBool("isDead", value); }
    }

    public void RequestAttack()
    {
        //Debug.Log("Attack requested");
        //nextAttackButtonPressed = false;

        //The character is not attacking and combo needs to start
        if (!AttackingStatus)
        {
            Debug.Log(AttackStates[0] + "FIRST PRESS");
            myAnimator.Play(AttackStates[0], 0);
        }
        //If character is already attacking, input is registered to act when current animation is finished
        else
        {
            myAnimator.SetTrigger("comboButtonPressed");
            Debug.Log("COMBO PRESS");
        }
    }
}