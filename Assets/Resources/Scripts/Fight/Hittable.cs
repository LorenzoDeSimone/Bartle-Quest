using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]

public class Hittable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int previousUpdateHealth;
    private int currentHealth;
    private bool justHit;

    private CharacterStatus myCharacterStatus;

    public bool JustHit
    {
        get { return justHit; }
    }

    // Use this for initialization
    void Start()
    {
        myCharacterStatus = GetComponent<CharacterStatus>();
        currentHealth = previousUpdateHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        justHit = previousUpdateHealth > currentHealth;
        previousUpdateHealth = currentHealth;
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public void UpdateHealth (int deltaHealth)
    {
        currentHealth = Mathf.Clamp(currentHealth + deltaHealth, 0, maxHealth);
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        myCharacterStatus.DeathStatus = true;
    }
}
