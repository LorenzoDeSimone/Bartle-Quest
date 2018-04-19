using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    //These are the real values that represent the player statistics
    //every method that wants to change character's equip permamently
    //should update through this class
    private static PlayerStatistics instance;
    private static PlayerStatistics rollbackInstance;

    [SerializeField] private WeaponInfo weapon;
    [SerializeField] private HeadInfo head;

    [SerializeField] private int maxHealth;
    private GameObject player;

    private EquipSlots playerEquipSlots;

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

    public HeadInfo PlayerHeadInfo
    {
        set
        {
            Instance().head = value;
            ChangePlayerHead();
        }

        get { return Instance().head; }
    }


    //Equips players with items he/she gathered so far
    void Awake()
    {
        Instance().player = FindObjectOfType<PlayerController>().gameObject;
        Instance().playerEquipSlots = Instance().player.GetComponent<EquipSlots>();
        LoadPlayerStatistics();

        rollbackInstance = (PlayerStatistics)instance.MemberwiseClone();
    }

    public void Rollback()
    {
        instance = rollbackInstance;
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
        Instance().playerEquipSlots.weapon.GetComponent<MeshFilter>().mesh = Instance().weapon.mesh;
        Instance().playerEquipSlots.weapon.damage = Instance().weapon.damage;
    }

    public void ChangePlayerWeapon(WeaponInfo weapon)
    {
        PlayerWeaponInfo = weapon;
        ChangePlayerWeapon();
    }

    private void ChangePlayerHead()
    {
        Instance().playerEquipSlots.head.GetComponent<MeshFilter>().mesh = Instance().head.mesh;
    }

    public void ChangePlayerHead(HeadInfo head)
    {
        PlayerHeadInfo = head;
        ChangePlayerHead();
    }

    public void GiveHealthBonus(int bonus)
    {
        Instance().maxHealth += bonus;
        ChangePlayerMaxHealth();
        Instance().player.GetComponent<Hittable>().CurrentHealth = Instance().maxHealth;
    }
}
