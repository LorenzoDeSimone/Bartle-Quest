using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGiver : MonoBehaviour
{
    [SerializeField] private ShieldInfo shield;

    public void GivePlayerShield()
    {
        PlayerStatistics.Instance().ChangePlayerShield(shield);
    }
}
