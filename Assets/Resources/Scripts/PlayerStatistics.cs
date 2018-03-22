using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    //These are the real values that represent the player statistics
    //every method that wants to change character's equip permamently
    //should update through this class

    [SerializeField] private WeaponInfo weapon;
    [SerializeField] private int maxHealth;
    private GameObject player;

    
    [SerializeField] private Weapon playerWeaponSlot;
    //[SerializeField] private Shield playerShieldSlot;

    public WeaponInfo PlayerWeaponInfo
    {
        set
        {
            weapon = value;
            ChangePlayerWeapon();
        }

        get { return weapon; }
    }

    //Equips players with items he/she gathered so far
    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        LoadPlayerStatistics();
    }

    private void LoadPlayerStatistics()
    {
        ChangePlayerMaxHealth();
        ChangePlayerWeapon();
    }

    public void ChangePlayerMaxHealth()
    {
        player.GetComponent<Hittable>().MaxHealth = maxHealth;
    }

    public void ChangePlayerWeapon()
    {
        playerWeaponSlot.GetComponent<MeshFilter>().mesh = weapon.mesh;
        playerWeaponSlot.damage = weapon.damage;
    }
}
