using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private CharacterStatus shieldHolder;
    [SerializeField] public GameObject blockEffect;

    public CharacterStatus GetShieldHolder()
    {
        return shieldHolder;
    }

    public void ActivateBlockEffect()
    {
        blockEffect.GetComponent<ParticleSystem>().Play();
        Debug.Log("www");
    }
}
