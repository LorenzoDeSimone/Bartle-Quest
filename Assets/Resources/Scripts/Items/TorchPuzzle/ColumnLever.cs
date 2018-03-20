using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnLever : Interactable
{
    [SerializeField] private TorchColumn column;
    private float animationSpeed;
    private Animation animation;
    private bool canInteract = true;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public override void Interact()
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
        animation.Play();
        column.clockwise = !column.clockwise;
    }

}
