using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatus))]

public class Hittable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    private int previousUpdateHealth;
    protected int currentHealth;
    private bool justHit    = false;
    private bool invincible = false;

    private CharacterStatus myCharacterStatus;

    public bool JustHit
    {
        get { return justHit; }
    }

    public bool Invincible
    {
        set { invincible = value; }
        get { return invincible; }
    }

    // Use this for initialization
    protected void Start()
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
        set { currentHealth = value; }
        get { return currentHealth; }
    }

    public virtual void UpdateHealth (int deltaHealth)
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
