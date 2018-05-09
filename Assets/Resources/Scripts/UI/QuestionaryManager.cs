using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionaryManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform results;
    [SerializeField] private Text demoAchieverText, demoExplorerText, demoSocializerText, demoKillerText;
    [SerializeField] private Text questionaryAchieverText, questionaryExplorerText, questionarySocializerText, questionaryKillerText;
    [SerializeField] Image DeathAndPauseScreen;

    private float fadeInTime = 0.5f;
    private float fadeOutTime = 1f;
    private Dictionary<BartleStatistics.ARCHETYPE, float> demoBartleStatistics, questionaryBartleStatistics;
    private GoogleDataSender googleDataSender;
    private bool chosenDemoProfile;

    // Use this for initialization
    void Start ()
    {
        results.gameObject.SetActive(false);
        googleDataSender = GetComponent<GoogleDataSender>();
        demoBartleStatistics = BartleStatistics.Instance().GetResults();
        BartleStatistics.Instance().Reset();
        StartCoroutine(FadeIn());
	}

    public void ShowResults()
    {
        questionaryBartleStatistics = BartleStatistics.Instance().GetResults();
        BartleStatistics.Instance().Reset();

        questionaryAchieverText.text   = (questionaryBartleStatistics[BartleStatistics.ARCHETYPE.ACHIEVER]   * 200).ToString("F2") + "%";
        questionaryExplorerText.text   = (questionaryBartleStatistics[BartleStatistics.ARCHETYPE.EXPLORER]   * 200).ToString("F2") + "%";
        questionarySocializerText.text = (questionaryBartleStatistics[BartleStatistics.ARCHETYPE.SOCIALIZER] * 200).ToString("F2") + "%";
        questionaryKillerText.text     = (questionaryBartleStatistics[BartleStatistics.ARCHETYPE.KILLER]     * 200).ToString("F2") + "%";

        demoAchieverText.text   = (demoBartleStatistics[BartleStatistics.ARCHETYPE.ACHIEVER]   * 200).ToString("F2") + "%";
        demoExplorerText.text   = (demoBartleStatistics[BartleStatistics.ARCHETYPE.EXPLORER]   * 200).ToString("F2") + "%";
        demoSocializerText.text = (demoBartleStatistics[BartleStatistics.ARCHETYPE.SOCIALIZER] * 200).ToString("F2") + "%";
        demoKillerText.text     = (demoBartleStatistics[BartleStatistics.ARCHETYPE.KILLER]     * 200).ToString("F2") + "%";

        results.gameObject.SetActive(true);
    }

    public void ChosenDemoProfile(bool value)
    {
        chosenDemoProfile = value;

        //Demo profile chosen over Bartle test
        if (value)
            DataSendProcedure(demoBartleStatistics);
        else
            DataSendProcedure(questionaryBartleStatistics);
    }

    private Dictionary<string, float[]> GetLevelLabeling()
    {
        //I know, this shouldn't be hardcoded. Don't give me that look. I know.
        Dictionary<string, float[]> levelLabeling = new Dictionary<string, float[]>();

        /* In order:
        Exploration       & Discovery
        Killing           & Destruction
        Escort            & Cooperation
        Tool              & Experimentation
        Dialogue          & Storytelling
        Collection        & Farm
        World Analysis    & Riddle
        Create            & Craft
        Competition       & Speedrun
        Outpost Upgrade   & Decorate
        */

        levelLabeling["Level1"] = new float[] { 0.5f , 0.25f, 0f   , 0f   , 0.25f, 0f   , 0.5f , 0f   , 0f   , 0f};
        levelLabeling["Level2"] = new float[] { 0.75f, 0.5f , 0f   , 0f   , 0.5f , 0f   , 0.5f , 0f   , 0f   , 0f };
        levelLabeling["Level3"] = new float[] { 0f   , 0f   , 0f   , 1f   , 1f   , 0f   , 1f   , 0f   , 0f   , 0f };
        levelLabeling["Level4"] = new float[] { 0f   , 1f   , 0.25f, 0f   , 0.25f, 0f   , 0f   , 0f   , 0f   , 0f };
        levelLabeling["Level5"] = new float[] { 0f   , 1f   , 0f   , 0f   , 0.5f , 0.5f , 0.25f, 0f   , 0.5f , 0f };
        levelLabeling["Level6"] = new float[] { 0f   , 0.5f , 0f   , 0f   , 0.5f , 0f   , 1f   , 0f   , 0f   , 0f };
        levelLabeling["Level7"] = new float[] { 0.25f, 0.5f , 0f   , 0f   , 0.75f, 0f   , 0.75f, 0f   , 0f   , 0f };
        levelLabeling["Level8"] = new float[] { 0f   , 1f   , 0.5f , 0f   , 0.25f, 0f   , 0f   , 0f   , 0f   , 0f };

        return levelLabeling;
    }

    private void DataSendProcedure(Dictionary<BartleStatistics.ARCHETYPE, float> bartleStatistics)
    {
        Dictionary<string, float> levelRatings = PlayerChoices.Instance().LevelRatings;

        Dictionary<string, float[]> levelLabeling = GetLevelLabeling();
      
        foreach (string levelName in levelRatings.Keys)
        {
            List<IList<object>> objNewRecords = new List<IList<object>>();
            IList<object> obj = new List<object>();

            obj.Add(bartleStatistics[BartleStatistics.ARCHETYPE.ACHIEVER]);
            obj.Add(bartleStatistics[BartleStatistics.ARCHETYPE.EXPLORER]);
            obj.Add(bartleStatistics[BartleStatistics.ARCHETYPE.SOCIALIZER]);
            obj.Add(bartleStatistics[BartleStatistics.ARCHETYPE.KILLER]);
            obj.Add(levelRatings[levelName]);
            obj.Add(levelName);

            //Right after the levelName, its labeling for each category
            foreach (float label in levelLabeling[levelName])
                obj.Add(label);

            if(chosenDemoProfile)
                obj.Add(1);
            else
                obj.Add(0);

            objNewRecords.Add(obj);

            //Sends one row
            googleDataSender.SendData(objNewRecords);
        }
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(FadeOut("MainMenu"));
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeInTime);
            DeathAndPauseScreen.color = new Color(DeathAndPauseScreen.color.r, DeathAndPauseScreen.color.g, DeathAndPauseScreen.color.b, newAlpha);
            yield return null;
        }
        dialogueManager.InitDialogue(GetComponent<Talker>());
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

        //Resets everything for a new game
        PlayerChoices.Instance().Reset();
        PlayerStatistics.Instance().Reset();
        BartleStatistics.Instance().Reset();     

        SceneManager.LoadScene(sceneName);
    }
}
