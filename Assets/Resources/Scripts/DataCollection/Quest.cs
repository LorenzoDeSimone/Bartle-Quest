using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest")]
public class Quest : ScriptableObject
{
    public string sceneName;

    public float EXPLORATION_DISCOVERY    = 0;
    public float KILLING_DESTRUCTION      = 0;
    public float ESCORT_COOP              = 0;
    public float TOOL_EXPERIMENTATION     = 0;
    public float DIALOGUE_STORY           = 0;
    public float COLLECT_FARM             = 0;
    public float WORLD_ANALYSIS_RIDDLE    = 0;
    public float CREATION_CRAFT           = 0;
    public float COMPETITION_SPEEDRUN     = 0;
    public float OUTPOST_UPGRADE_DECORATE = 0;
}
