using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    public bool IsEnemy
    {
        get { return  isEnemy; }
        set { isEnemy = value; }
    }

}
