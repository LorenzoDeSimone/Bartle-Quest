using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus shieldHolder;

    public CharacterStatus GetShieldHolder()
    {
        return shieldHolder;
    }
}
