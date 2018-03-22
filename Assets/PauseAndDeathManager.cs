using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseAndDeathManager : MonoBehaviour
{
    private UIPlayerHealth uIPlayerHealth;
    [SerializeField] Image DeathAndPauseScreen;
    private float elapsedTime = 0;
    private float fadeInTime = 0.5f;
    private float fadeOutTime = 1f;

    // Use this for initialization
    void Start()
    {
        uIPlayerHealth = GetComponent<UIPlayerHealth>();
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update ()
    {
        if (uIPlayerHealth.playerHittable.CurrentHealth == 0)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        elapsedTime = 0;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        elapsedTime = 0;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutTime);
            Debug.Log(newAlpha);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
