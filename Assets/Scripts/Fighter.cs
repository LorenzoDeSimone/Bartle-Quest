using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField]
    private int health = 1;

    // Use this for initialization
    void Start ()
    {
    		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Hit (int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    private void Die()
    {

    }
}
