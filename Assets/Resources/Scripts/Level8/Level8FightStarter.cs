using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8FightStarter : MonoBehaviour
{
    [SerializeField] private GameObject master;
    [SerializeField] private GameObject barrier;

	public void StartFight()
    {
        master.SetActive(true);
        barrier.SetActive(true);
    }

}
