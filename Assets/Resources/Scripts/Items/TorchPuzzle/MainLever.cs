using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLever : Interactable
{
    [SerializeField] private TorchColumn[] columns;

    private Animation animation;
    private bool canInteract = true;
    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public override void Interact()
    {
        if (!canInteract)
            return;

        canInteract = false;

        /*
        if (animationSpeed == 1)
        {
            animation["PullLever"].time = animation["PullLever"].length;
            animationSpeed = -1f;
        }
        else
        {
            animation["PullLever"].time = 0f;
            animationSpeed = 1;
        }*/

        //animation["PullLever"].speed = animationSpeed;
        //animationSpeed -= animationSpeed; 
        //animation.Play();
        StartCoroutine(Pull());
        foreach (TorchColumn t in columns)
        {
            t.Rotate();
        }
    }

    private IEnumerator Pull()
    {
        //Pull
        animation.Play();
        while (animation.isPlaying)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        animation["PullLever"].time = animation["PullLever"].length;
        animation["PullLever"].speed = -1f;
        animation.Play();

        //Come back
        while (animation.isPlaying)
            yield return null;

        animation["PullLever"].speed = 1f;
        canInteract = true;
    }
}


/*    public override void Interact()
    {
        if (animation.isPlaying)
            return;

        if (animationSpeed == 1)
        {
            animation["PullLever"].time = animation["PullLever"].length;
            animationSpeed = -1f;
        }
        else
        {
            animation["PullLever"].time = 0f;
            animationSpeed = 1;
        }

        animation["PullLever"].speed = animationSpeed;
        //animationSpeed -= animationSpeed; 
        animation.Play();
        foreach(TorchColumn t in columns)
        {
            t.Rotate();
        }
    }*/
