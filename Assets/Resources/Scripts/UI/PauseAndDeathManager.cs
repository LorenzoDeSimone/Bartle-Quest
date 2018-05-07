using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseAndDeathManager : MonoBehaviour
{
    private UIPlayerHealth uIPlayerHealth;
    [SerializeField] Image DeathAndPauseScreen;
    [SerializeField] private bool pauseEnabled = true;

    private float elapsedTime = 0;
    private float fadeInTime = 0.5f;
    private float fadeOutTime = 1f;

    private bool pauseButtonToggle = false;

    private static PauseAndDeathManager instance;

    // Use this for initialization
    void Start()
    {
        uIPlayerHealth = GetComponent<UIPlayerHealth>();
        StartCoroutine(FadeIn());
    }

    public static PauseAndDeathManager Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<PauseAndDeathManager>();

        return instance;
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsPlayerDead())
            ReloadScene();
        else if(pauseEnabled)
            PauseManagement();
    }

    private void ResetPlayerInfo()
    {
        PlayerChoices.Instance().Rollback();
        PlayerStatistics.Instance().Rollback();
        BartleStatistics.Instance().Rollback();
    }

    private bool IsPlayerDead()
    {
        return uIPlayerHealth.playerHittable.CurrentHealth == 0;
    }

    private void PauseManagement()
    {
        if (Time.timeScale > 0f && Input.GetButtonDown("Start") && !pauseButtonToggle)
        {
            pauseButtonToggle = true;
            Time.timeScale = 0f;
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, 0.8f);
            DeathAndPauseScreen.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
            FindObjectOfType<PlayerController>().GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        }
        else if (Input.GetButtonDown("Start") && pauseButtonToggle)
        {
            pauseButtonToggle = false;
            Time.timeScale = 1f;
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, 0);
            DeathAndPauseScreen.GetComponentInChildren<Text>(true).gameObject.SetActive(false);
            FindObjectOfType<PlayerController>().GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
        }
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

    IEnumerator FadeOut(string sceneName)
    {
        elapsedTime = 0;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }        
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        DeathAndPauseScreen.GetComponentInChildren<Text>(true).gameObject.SetActive(false);
        ResetPlayerInfo();
        StartCoroutine(FadeOut(SceneManager.GetActiveScene().name));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }
}
