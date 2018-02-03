using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]

public class Hittable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;
    private int currHealth;

    private CharacterStatus myCharacterStatus;

    // Use this for initialization
    void Start ()
    {
        myCharacterStatus = GetComponent<CharacterStatus>();
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void Hit (int damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
            Die();
    }

    private void Die()
    {
        myCharacterStatus.DeathStatus = true;
    }
}
