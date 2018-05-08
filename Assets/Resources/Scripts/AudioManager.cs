using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioSource aSource;

    private static AudioManager instance;

    public AudioSource Asource
    {
        get { return Instance().aSource; }
    }

	// Use this for initialization
	void Start ()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<AudioManager>();
            instance.aSource = instance.GetComponent<AudioSource>();
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
        aSource.PlayOneShot(success);
    }

    public void PlayClipOneShot(AudioClip clip, float volume, bool waitSound = false)
    {
        if (waitSound && aSource.isPlaying)
            return;

        aSource.PlayOneShot(clip, volume);
    }

    public void PlayClipOneShot(AudioClip clip, bool waitSound=false)
    {
        PlayClipOneShot(clip, 1.0f, waitSound);
    }
}
