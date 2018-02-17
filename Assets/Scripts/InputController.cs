using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingCharacter))]

public class InputController : MonoBehaviour
{
    private float forwardInput, turnInput;

    private MovingCharacter myMovingCharacter;

    // Use this for initialization
    void Start ()
    {
        forwardInput = turnInput = 0;
        myMovingCharacter = GetComponent<MovingCharacter>();
    }

    // Update is called once per frame
    void Update ()
    {
        GetInput();
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if(Input.GetButtonDown("Attack"))
            myMovingCharacter.Attack();

        //if (Input.GetButtonDown("Jump"))
        //    myMovingCharacter.Jump();
    }
}
