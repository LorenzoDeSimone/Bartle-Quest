using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlesser : MonoBehaviour
{
    [SerializeField] Transform playerWeapon;
    [SerializeField] GameObject lightEffect;
    [SerializeField] Talker ghost;

    public void BlessWeapon()
    {
        GameObject go = Instantiate<GameObject>(lightEffect);
        lightEffect.GetComponent<Weapon>().weaponHolder = playerWeapon.GetComponent<Weapon>().weaponHolder;
        go.transform.SetParent(playerWeapon);
        go.transform.localPosition = new Vector3(0f, 0f, 1.5f);
        go.transform.localRotation = Quaternion.Euler(-25f, 90f, 0f);
    }

    public void ChangeGhostDialogue(string dialogueName)
    {
        ghost.DialogueName = dialogueName;
    }
}
