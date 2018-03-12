using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]

public class Hittable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 1;
    private int previousUpdateHealth;
    [SerializeField] private int currHealth;
    private bool justHit;

    private CharacterStatus myCharacterStatus;

    public bool JustHit
    {
        get { return justHit; }
    }

    // Use this for initialization
    void Start ()
    {
        myCharacterStatus = GetComponent<CharacterStatus>();
        currHealth = previousUpdateHealth = maxHealth;
    }

    // Update is called once per frame
    void Update ()
    {
        justHit = previousUpdateHealth < currHealth;
        previousUpdateHealth = currHealth;
	}

    public void UpdateHealth (int deltaHealth)
    {
        currHealth += deltaHealth;
        if (currHealth <= 0)
            Die();
    }

    private void Die()
    {
        myCharacterStatus.DeathStatus = true;
    }
}
