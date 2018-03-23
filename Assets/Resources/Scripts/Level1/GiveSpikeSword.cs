using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSpikeSword : MonoBehaviour
{
    [SerializeField] private WeaponInfo weapon;
    [SerializeField] private GameObject SpikeWeapon;

    public void GivePlayerSword()
    {
        PlayerStatistics.Instance().ChangePlayerWeapon(weapon);
        SpikeWeapon.SetActive(false);
    }
}
