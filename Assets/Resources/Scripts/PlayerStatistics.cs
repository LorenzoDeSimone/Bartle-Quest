using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    //These are the real values that represent the player statistics
    //every method that wants to change character's equip permamently
    //should update through this class
    private static PlayerStatistics instance;

    [SerializeField] private WeaponInfo weapon;
    [SerializeField] private int maxHealth;
    private GameObject player;

    [SerializeField] private Weapon playerWeaponSlot;
    //[SerializeField] private Shield playerShieldSlot;

    public static PlayerStatistics Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<PlayerStatistics>();

        return instance;
    }

    public WeaponInfo PlayerWeaponInfo
    {
        set
        {
            Instance().weapon = value;
            ChangePlayerWeapon();
        }

        get { return Instance().weapon; }
    }

    //Equips players with items he/she gathered so far
    void Start()
    {
        Instance().player = FindObjectOfType<PlayerController>().gameObject;
        LoadPlayerStatistics();
    }

    private void LoadPlayerStatistics()
    {
        ChangePlayerMaxHealth();
        ChangePlayerWeapon();
    }

    public void ChangePlayerMaxHealth()
    {
        Instance().player.GetComponent<Hittable>().MaxHealth = Instance().maxHealth;
    }

    private void ChangePlayerWeapon()
    {
        Instance().playerWeaponSlot.GetComponent<MeshFilter>().mesh = Instance().weapon.mesh;
        Instance().playerWeaponSlot.damage = Instance().weapon.damage;
    }

    public void ChangePlayerWeapon(WeaponInfo weapon)
    {
        PlayerWeaponInfo = weapon;
        ChangePlayerWeapon();
    }
}
