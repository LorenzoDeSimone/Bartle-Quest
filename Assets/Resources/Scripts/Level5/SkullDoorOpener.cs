using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoorOpener : Interactable
{
    [SerializeField] private Transform skullLight;
    [SerializeField] private ExplodingDoor[] doors;

    private static int nLevers = 0;
    private static int nLeversPulled;

    private bool leverPulled = false;
    private Animation animation; 

    void Awake()
    {
        nLevers++;
        animation = GetComponent<Animation>();
    }

    public override bool CanInteract()
    {
        return !leverPulled;
    }

    public override void Interact()
    {
        leverPulled = true;
        animation.Play();
        skullLight.gameObject.SetActive(true);

        nLeversPulled++;
        if (nLeversPulled >= nLevers)
        {
            foreach (ExplodingDoor door in doors)
            {
                if (door)
                   door.Explode();
            }
        }
    }
}