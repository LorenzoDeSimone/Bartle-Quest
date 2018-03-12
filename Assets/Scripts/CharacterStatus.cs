using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] private bool canUseShield;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canFall;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool canDie;

    [SerializeField] private string[] AttackStates;
    public static readonly int movingIdleValue = 0, movingWalkValue = 1, movingRunValue = 2;

    [SerializeField] private bool isNPC;
    [SerializeField] private Animator AIManager;

    private Animator myAnimator;

    private bool shieldUpThisFrame;
    private float currAnimationLenght;
    private int lastAnimatorState;
    
    private HashSet<GameObject> hitEnemies;

    // Use this for initialization
    protected void Start()
    {
        hitEnemies = new HashSet<GameObject>();
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
            Debug.LogError("No animator!");
        if(isNPC && AIManager==null)
            Debug.LogError("No AI Manager for an NPC!");
    }

    public bool ShieldUpStatus
    {
        get
        {
            if (!canUseShield)
                return false;

            return myAnimator.GetBool("isShieldUp");
        }
        set
        {
            if (!canUseShield)
                return;

            myAnimator.SetBool("isShieldUp", value);
        }
    }

    public int MovingStatus
    {
        get
        {
            if (!canMove)
                return -1;

            return myAnimator.GetInteger("isMoving");
        }
        set
        {
            if (!canMove)
                return;

            myAnimator.SetInteger("isMoving", value);
        }
    }


    public bool GroundedStatus
    {
        get
        {
            if (!canMove)
                return true;

            return myAnimator.GetBool("isGrounded");
        }
        set
        {
            if (!canMove)
                return;

            if (myAnimator.GetBool("isGrounded") && !value)
            {
                //Debug.Log("aa");
                myAnimator.SetTrigger("isFalling");
            }

            myAnimator.SetBool("isGrounded", value);
        }
    }

    public bool CanWeaponHit(GameObject enemy)
    {
        return !hitEnemies.Contains(enemy);
    }

    public void AddHitEnemy(GameObject enemy, bool shieldHit = false)
    {
        hitEnemies.Add(enemy);
        if (shieldHit)
            myAnimator.SetTrigger("attackBlocked");
    }

    public void RemoveAllHitEnemies()
    {
        hitEnemies.Clear();
        myAnimator.ResetTrigger("attackBlocked");
    }

    public bool AttackingStatus
    {
        //Checks if the character is in one of the states labelled as attackStates in the animator
        get
        {
            if (!canAttack)
                return false;

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
        get
        {
            if (!canDie)
                return false;

            return myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death");
        }
        set
        {
            if (!canDie)
                return;

            if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                myAnimator.SetTrigger("isDead");
        }
    }

    public void RequestAttack()
    {
        if (!canAttack)
            return;

        //The character is not attacking and combo needs to start
        if (!AttackingStatus)
            myAnimator.SetTrigger("firstAttack");
        else
            myAnimator.SetTrigger("comboButtonPressed");
    }

    public void AttackBlocked()
    {
        if (!AttackingStatus)
            return;
        else
        {
            if(isNPC)
                FSM.SetTrigger("attackBlocked");

            myAnimator.SetTrigger("attackBlocked");
        }
    }

    //Method used in animation event
    public void DisableTransitionsToFalse()
    {
        myAnimator.SetBool("disableTransitions", false);
    }

    public Animator FSM
    {
        get
        {
            if (isNPC)
                return AIManager;
            else
                return null;
        }
    }

}