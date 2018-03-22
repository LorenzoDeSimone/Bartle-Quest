using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSword : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private PlayerStatistics playerStatistics;

	// Use this for initialization
	void Start ()
    {
        playerStatistics = FindObjectOfType<PlayerStatistics>();	
	}

    public void GivePlayerSword()
    {
        //playerStatistics.ChangePlayerWeapon(weapon);
    }
}
