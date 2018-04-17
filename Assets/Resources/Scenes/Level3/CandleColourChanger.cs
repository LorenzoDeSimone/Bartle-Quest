using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleColourChanger : Interactable
{
    [SerializeField] PlayerCandle playerCandle;
    [SerializeField] private PlayerCandle.CANDLE_COLOUR colour;

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        playerCandle.AddColour(colour);
    }
}
