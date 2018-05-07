using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    private static AudioManager instance;
    private AudioSource success;//, death, weapon1, weapon2, weapon3, footsteps;

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioManager>();
            var aSources = GetComponents<AudioSource>();
            success = aSources[0];
     
        }
	}
	
    public static AudioManager Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<AudioManager>();
        return instance;
    }

    public void PlaySuccess()
    {
        success.Play();
    }
}
