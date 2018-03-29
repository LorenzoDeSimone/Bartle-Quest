using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DataSender : MonoBehaviour
{
    public void SendData()
    {
        Dictionary<string, object> questEvaluationData = new Dictionary<string, object>();
        questEvaluationData.Add("Quest Name", "Level1");
        questEvaluationData.Add("Achiever"  , 0.25f);
        questEvaluationData.Add("Explorer"  , 0f);
        questEvaluationData.Add("Socializer", 0.25f);
        questEvaluationData.Add("Killer"    , 0.5f);
        questEvaluationData.Add("Reward"    , 0.8f);

        Analytics.CustomEvent("Quest Evaluation", questEvaluationData);
    }
}
