using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level7DoorLever : Interactable
{
    [SerializeField] private Transform prisoners;
    [SerializeField] private Transform prisonersFinalPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] Image DeathAndPauseScreen;
    [SerializeField] private GameObject[] Enemies;

    private float fadeInTime = 0.5f;
    private float fadeOutTime = 1f;

    private Animation animation;

    private bool leverPulled = false;

    void Start()
    {
        animation = GetComponent<Animation>();
    }

    public override void Interact()
    {
        animation.Play();
        leverPulled = true;
        StartCoroutine(DialogueFade());
    }

    public override bool CanInteract()
    {
        if (leverPulled)
        {
            return false;
        }

        /*foreach (GameObject go in Enemies)
        {
            if (go != null)
            {
                return false;
            }
        }*/

        return true;
    }

    IEnumerator DialogueFade()
    {
        float elapsedTime = 0;

        //FadeOut
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }

        //In between changes
        prisoners.position = prisonersFinalPosition.position;

        //FadeIn
        elapsedTime = 0;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }

        dialogueManager.InitDialogue(GetComponent<Talker>());
    }

}

