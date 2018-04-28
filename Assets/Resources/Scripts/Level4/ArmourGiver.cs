using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourGiver : MonoBehaviour
{
    [SerializeField] private ArmourInfo Armour;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void GivePlayerArmour ()
    {
        PlayerStatistics.Instance().ChangePlayerArmour(Armour);
    }
}
