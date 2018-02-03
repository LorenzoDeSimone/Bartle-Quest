using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]


public class CharacterStatus : MonoBehaviour
{
    private bool _isRunning = false;
    private bool _isAttacking = false;

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
        get { return _isAttacking; }
        set
        {
            _isAttacking = value;
            myAnimator.SetBool("isAttacking", _isAttacking);
        }
    }

}