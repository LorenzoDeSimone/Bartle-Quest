using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSwitch : Interactable
{
   [SerializeField] private GameObject myWalls;
    [SerializeField] private bool activeOnStart = true;

    void Awake()
    {
        myWalls.SetActive(activeOnStart);
    }

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        myWalls.SetActive(!myWalls.active);
    }
}
