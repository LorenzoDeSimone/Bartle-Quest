using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip success, ui_ok;

    private AudioSource aSource;

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

    public void PlayUISelect()
    {
        aSource.PlayOneShot(ui_ok);
    }

    public void PlayClipOneShot(AudioClip clip, float volume)
    {
        aSource.PlayOneShot(clip, volume);
    }

    public void PlayClipOneShot(AudioClip clip)
    {
        PlayClipOneShot(clip, 1.0f);
    }
}
