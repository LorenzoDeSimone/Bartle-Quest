using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipChanger : MonoBehaviour
{
    [SerializeField] private Weapon playerWeapon;
    [SerializeField] private HeadGear playerHeadgear;
    [SerializeField] private Shield playerShield;
    [SerializeField] private BodyGear playerBodygear;

    public void ChangeWeapon(Weapon weapon, Mesh weaponMesh)
    {
        playerWeapon = weapon;
        playerWeapon.GetComponent<MeshFilter>().mesh = weaponMesh;
    }
}
