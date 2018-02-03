using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterStatus : MonoBehaviour
{

    [SerializeField]
    private string Attack1, Attack2, Attack3;


    private bool _isRunning = false;
    private bool _isAttacking = false;
    private bool _isDead = false;


    private bool nextAttackButtonPressed = false;

    private Animator myAnimator;

    // Use this for initialization
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    public bool RunningStatus
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            myAnimator.SetBool("isRunning", _isRunning);
        }
    }

    public bool AttackingStatus
    {
        get { return myAnimator.GetCurrentAnimatorStateInfo(0).IsName(Attack1); }
    }

    public void RequestAttack()
    {
        //Debug.Log("Attack requested");
        //nextAttackButtonPressed = false;
        if (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName(Attack1))//!myAnimator.GetCurrentAnimatorStateInfo(0).IsName(Attack1))
        {
            Debug.Log("FIRST PRESS");
            //_isAttacking = true;
            //StartCoroutine(PlayOneShot("isAttacking"));
            //myAnimator.SetBool("isAttacking", _isAttacking);
            //myAnimator.SetTrigger("isAttacking");
            myAnimator.Play(Attack1, 0);
            //StartCoroutine(wait("Attack02"));
        }

        //_isAttacking = false;
        //myAnimator.SetBool("isAttacking", false);

        //If character is already attacking, input is registered to act when current animation is finished

        else//if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName(Attack1))
        {
            nextAttackButtonPressed = true;
            Debug.Log("SECOND PRESS");
        }
    }

    void Update()
    {
        //_isAttacking = false;
        //Debug.Log(nextAttackButtonPressed);
    }

    public IEnumerator PlayOneShot(string paramName)
    {
        myAnimator.SetBool(paramName, true);
        yield return null;
        myAnimator.SetBool(paramName, false);
        _isAttacking = false;
    }


    IEnumerator wait(string animationName)
    {
        //yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length + 
        //myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))
        while (!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            yield return null;

        /*while ((!myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack02"))||
              (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !myAnimator.IsInTransition(0)))
        {
            yield return null;
        }*/

        _isAttacking = false;
        myAnimator.SetBool("isAttacking", _isAttacking);
    }

    public bool DeathStatus
    {
        get { return _isDead; }
        set
        {
            _isDead = value;
            myAnimator.SetBool("isDead", _isDead);
        }
    }

}