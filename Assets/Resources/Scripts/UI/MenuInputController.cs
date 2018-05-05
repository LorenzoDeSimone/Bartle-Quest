using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInputController : MonoBehaviour
{
    [SerializeField] private Light maleLight, femaleLight;
    [SerializeField] private HeadInfo maleHead, femaleHead;
    [SerializeField] private string Level1Name;
    [SerializeField] private float lightIntensity;
    [SerializeField] Image DeathAndPauseScreen;
    private float fadeOutTime = 1f;

    void Start()
    {
        femaleLight.intensity = 0;
        maleLight.intensity = lightIntensity;
        PlayerChoices.Instance().IsMale = true;
    }

    void Update ()
    {
        if (Input.GetButtonDown("A"))
            StartCoroutine(FadeOut(Level1Name));
        else if (Input.GetButtonDown("Y"))
            Application.Quit();
        else if (Input.GetAxis("RT") > 0.2f)
        {
            femaleLight.intensity = lightIntensity;
            maleLight.intensity = 0;
            PlayerChoices.Instance().IsMale = false;
            PlayerStatistics.Instance().ChangePlayerHead(femaleHead);
        }
        else if (Input.GetAxis("LT") > 0.2f)
        {
            femaleLight.intensity = 0;
            maleLight.intensity = lightIntensity;
            PlayerChoices.Instance().IsMale = true;
            PlayerStatistics.Instance().ChangePlayerHead(maleHead);
        }

    }


    IEnumerator FadeOut(string sceneName)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
