using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class QuestionaryManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Transform results;
    [SerializeField] private Text demoAchieverText, demoExplorerText, demoSocializerText, demoKillerText;
    [SerializeField] private Text questionaryAchieverText, questionaryExplorerText, questionarySocializerText, questionaryKillerText;

    private Dictionary<BartleStatistics.ARCHETYPE, float> demoBartleStatistics, questionaryBartleStatistics;
    private GoogleDataSender googleDataSender;

    // Use this for initialization
    void Start ()
    {
        results.gameObject.SetActive(false);
        googleDataSender = GetComponent<GoogleDataSender>();
        demoBartleStatistics = BartleStatistics.Instance().GetResults();
        BartleStatistics.Instance().Reset();
        dialogueManager.InitDialogue(GetComponent<Talker>());
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
        //Demo profile chosen over Bartle test
        if (value)
            DataSendProcedure(demoBartleStatistics);
        else
            DataSendProcedure(questionaryBartleStatistics);
    }

    private void DataSendProcedure(Dictionary<BartleStatistics.ARCHETYPE, float> bartleStatistics)
    {
        Dictionary<string, float> levelRatings = PlayerChoices.Instance().LevelRatings;
        //levelRatings = new Dictionary<string, float>();
        //levelRatings["Level1"] = 0.1f;
        //levelRatings["Level2"] = 0.2f;
        //levelRatings["Level3"] = 0.3f;
        //levelRatings["Level4"] = 0.4f;

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
            objNewRecords.Add(obj);

            //Sends one row
            googleDataSender.SendData(objNewRecords);
        }
    }
}
