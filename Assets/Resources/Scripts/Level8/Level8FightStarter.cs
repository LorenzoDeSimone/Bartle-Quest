using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8FightStarter : MonoBehaviour
{
    [SerializeField] private MasterStatus master;
    [SerializeField] private GameObject barrier;

	public void StartFight()
    {
        master.AIManager.enabled = true;
        barrier.SetActive(true);
    }
}
