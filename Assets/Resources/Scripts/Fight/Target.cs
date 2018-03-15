using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [SerializeField] private bool isEnemy;
    [SerializeField] private string text;
    [SerializeField] private Sprite buttonImage;

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

}
