using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level7DoorLever : Interactable
{
    [SerializeField] private Transform prisoners;
    [SerializeField] private Transform prisonersFinalPosition;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] Image DeathAndPauseScreen;

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
        UpdateArchetypes();
        StartCoroutine(DialogueFade());
    }

    public void UpdateArchetypes()
    {
        if (PlayerChoices.Instance().Lv7SkeletonControlled)
            BartleStatistics.Instance().IncrementSocializer();
        else if (PlayerChoices.Instance().Lv7AlarmTriggered)
            BartleStatistics.Instance().IncrementKiller();
        else
            BartleStatistics.Instance().IncrementExplorer();
    }

    public override bool CanInteract()
    {
        return !leverPulled;
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

