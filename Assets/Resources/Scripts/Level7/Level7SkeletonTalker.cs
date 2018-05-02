using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7SkeletonTalker : Talker
{
    public override bool CanInteract()
    {
        return !PlayerChoices.Instance().Lv7AlarmTriggered && !PlayerChoices.Instance().Lv7SkeletonControlled;
    }
}
