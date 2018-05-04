using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level8General : MonoBehaviour
{
    private bool isHelpingInFight = false;
    private GuardStatus guardStatus;

    public bool IsHelpingInFight
    {
        get { return isHelpingInFight; }
    }

	// Use this for initialization
	void Start ()
    {
        guardStatus = GetComponent<GuardStatus>();
	}
	
    public void HelpWithFight()
    {
        isHelpingInFight = true;
        guardStatus.AIManager.enabled = true;
    }
}
