using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadgearGiver : MonoBehaviour
{
    [SerializeField] private HeadInfo GogglesF, GogglesM, HatF, HatM;

    public void GivePlayerHeadgear(string choice)
    {
        if (choice.Equals("hat"))
        {
            if (PlayerChoices.Instance().IsMale)
                PlayerStatistics.Instance().ChangePlayerHead(HatM);
            else
                PlayerStatistics.Instance().ChangePlayerHead(HatF);
        }
        else if (choice.Equals("goggles"))
        {
            if (PlayerChoices.Instance().IsMale)
                PlayerStatistics.Instance().ChangePlayerHead(GogglesM);
            else
                PlayerStatistics.Instance().ChangePlayerHead(GogglesF);
        }
    }
}
