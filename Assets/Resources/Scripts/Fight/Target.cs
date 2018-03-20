﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField] private bool isEnemy;
    [SerializeField] private string text;
    [SerializeField] private string buttonName;
    [SerializeField] private Sprite buttonImage;

    [SerializeField] private bool dialogueInteraction;

    public string Text
    {
        get { return text; }
    }

    public Sprite Buttonimage
    {
        get { return buttonImage; }
    }

    public bool IsEnemy
    {
        get { return  isEnemy; }
        set { isEnemy = value; }
    }

    public void Interact(string buttonName)
    {
        if (this.buttonName.Equals(buttonName))
        {
            if (dialogueInteraction)
            {
                GameObject Canvas = GameObject.Find("CanvasPlayerUI");
                Canvas.GetComponent<DialogueManager>().InitDialogue(GetComponent<Talker>());
            }
            //else -> Future interaction types here
        }
    }
}