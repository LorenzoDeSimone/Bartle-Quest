using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterStatus : MonoBehaviour
{
    [SerializeField] protected bool canUseShield;
    [SerializeField] protected bool canMove;
    [SerializeField] protected bool canFall;
    [SerializeField] protected bool canAttack;
    [SerializeField] protected bool canDie;
    [SerializeField] protected bool canCollectItems;

    [SerializeField] private string[] AttackStates;
    [SerializeField] private AudioClip[] AttackSounds;
    [SerializeField] private AudioClip blockSound;
    [SerializeField] private AudioClip[] onHitSounds;
    [SerializeField] private AudioClip[] footstepSounds;

    public static readonly int movingIdleValue = 0, movingWalkValue = 1, movingRunValue = 2;

    [SerializeField] private bool isNPC;
    [SerializeField] private Animator aiManager;

    protected Animator myAnimator;

    private bool shieldUpThisFrame, alreadyDead;
    private float currAnimationLenght;
    private int lastAnimatorState;
    
    private HashSet<GameObject> hitEnemies;

    public Animator AIManager
    {
        get { return aiManager; }
    }
    
    public bool CanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    public bool CanAttack
    {
         set { canAttack = value; }
         get { return canAttack; }
    }

    // Use this for initialization
    protected void Start()
    {
        hitEnemies = new HashSet<GameObject>();
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
            Debug.LogError("No animator!");
        if(isNPC && aiManager==null)
            Debug.LogError("No AI Manager for an NPC!");
    }

    public bool CanCollectItems
    {
        get { return canCollectItems; }
    }

    public bool ShieldUpStatus
    {
        get
        {
            if (!canUseShield || !myAnimator)
                return false;

            return myAnimator.GetBool("isShieldUp");
        }
        set
        {
            if (!canUseShield || !myAnimator)
                return;

            myAnimator.SetBool("isShieldUp", value);
        }
    }

    public int MovingStatus
    {
        get
        {
            if (!canMove || !myAnimator)
                return -1;

            return myAnimator.GetInteger("isMoving");
        }
        set
        {
            if (!canMove || !myAnimator)
                return;

            myAnimator.SetInteger("isMoving", value);
        }
    }


    public bool GroundedStatus
    {
        get
        {
            if (!canMove || !myAnimator)
                return true;

            return myAnimator.GetBool("isGrounded");
        }
        set
        {
            if (!canMove || !myAnimator)
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
            if (!canAttack || !myAnimator)
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
            if (!canDie || !myAnimator)
                return false;

            return myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") || myAnimator.GetAnimatorTransitionInfo(0).IsName("AnyState -> Death");
        }
        set
        {
            if (!canDie || !myAnimator)
                return;

            if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death") && !alreadyDead)
            {
                alreadyDead = true;//Just needed for eventual animation transition time: avoids multiple death triggers in corner cases
                GetComponent<Collider>().enabled = false;
                myAnimator.SetTrigger("isDead");
            }
        }
    }

    public void RequestAttack()
    {
        //Debug.Log("canAttack: " + canAttack + " | myAnimator: " + myAnimator==null);

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
            if (isNPC)
            {
                FSM.SetTrigger("attackBlocked");
            }

            myAnimator.SetTrigger("attackBlocked");
        }
    }

    //Method used in animation event
    public void DisableTransitionsToFalse()
    {
        myAnimator.SetBool("disableTransitions", false);
    }

    public void PlayAttackSound(int i)
    {
        if (i >= 0 && i < AttackSounds.Length)
            AudioManager.Instance().Asource.PlayOneShot(AttackSounds[i]);
    }

    public void PlayBlockSound()
    {
        if (blockSound)
            AudioManager.Instance().Asource.PlayOneShot(blockSound);
    }

    public void PlayOnHitSound()
    {
        if (onHitSounds != null && onHitSounds.Length >= 1)
            AudioManager.Instance().Asource.PlayOneShot(onHitSounds[Random.Range(0, onHitSounds.Length - 1)]);
    }

    public void PlayFootStepSound(float intensity)
    {
        if (footstepSounds!=null && footstepSounds.Length >=1)
            AudioManager.Instance().Asource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length-1)], intensity);
    }

    public Animator FSM
    {
        get
        {
            if (isNPC)
                return aiManager;
            else
                return null;
        }
    }

}