using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredLibraryTalker : Talker
{
    [SerializeField] private PlayerCandle.CANDLE_COLOUR colour;
    [SerializeField] private PlayerCandle playerCandle;

    [SerializeField] private string ColourMatchDialogue;
    [SerializeField] private string ColourMismatchDialogue;

    public override void Interact()
    {
        if (colour.Equals(playerCandle.CurrentColour))
            DialogueName = ColourMatchDialogue;
        else
            DialogueName = ColourMismatchDialogue;

        base.Interact();
    }
}
