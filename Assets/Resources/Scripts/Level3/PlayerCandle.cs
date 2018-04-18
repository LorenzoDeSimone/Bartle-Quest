using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCandle : MonoBehaviour
{
    public enum CANDLE_COLOUR { UNLIT, RED, YELLOW, BLUE, ORANGE, GREEN, PURPLE }

    private CANDLE_COLOUR currentColour = CANDLE_COLOUR.UNLIT;
    private Dictionary<CANDLE_COLOUR, Color> enumToColour;
    private Dictionary<CANDLE_COLOUR, Gradient> enumToGradient;

    [SerializeField] private ParticleSystem candleParticleSystem;
    [SerializeField] private Light candleLight;
    [SerializeField] private float candleLightIntensity;

    public Color redLight, yellowLight, blueLight, orangeLight, greenLight, purpleLight;
    public Gradient redGradient, yellowGradient, blueGradient, orangeGradient, greenGradient, purpleGradient;

    void Start()
    {
        InitDictionaries();
    }

    public CANDLE_COLOUR CurrentColour
    {
        get { return currentColour; }
    }

    void InitDictionaries()
    {
        enumToColour   = new Dictionary<CANDLE_COLOUR, Color>();
        enumToGradient = new Dictionary<CANDLE_COLOUR, Gradient>();

        enumToColour.Add(CANDLE_COLOUR.RED, redLight);
        enumToGradient.Add(CANDLE_COLOUR.RED, redGradient);

        enumToColour.Add(CANDLE_COLOUR.YELLOW, yellowLight);
        enumToGradient.Add(CANDLE_COLOUR.YELLOW, yellowGradient);

        enumToColour.Add(CANDLE_COLOUR.BLUE, blueLight);
        enumToGradient.Add(CANDLE_COLOUR.BLUE, blueGradient);

        enumToColour.Add(CANDLE_COLOUR.ORANGE, orangeLight);
        enumToGradient.Add(CANDLE_COLOUR.ORANGE, orangeGradient);

        enumToColour.Add(CANDLE_COLOUR.GREEN, greenLight);
        enumToGradient.Add(CANDLE_COLOUR.GREEN, greenGradient);

        enumToColour.Add(CANDLE_COLOUR.PURPLE, purpleLight);
        enumToGradient.Add(CANDLE_COLOUR.PURPLE, purpleGradient);
    }

    public void AddColour(CANDLE_COLOUR COLOUR_B)
    {
        if (!currentColour.Equals(COLOUR_B))
        {
            if (currentColour.Equals(CANDLE_COLOUR.UNLIT)) //Simple Cases
                currentColour = COLOUR_B;
            else //Colour mix cases
            {
                if (currentColour.Equals(CANDLE_COLOUR.RED)  && COLOUR_B.Equals(CANDLE_COLOUR.BLUE) ||
                    currentColour.Equals(CANDLE_COLOUR.BLUE) && COLOUR_B.Equals(CANDLE_COLOUR.RED))
                    currentColour = CANDLE_COLOUR.PURPLE;
                else if (currentColour.Equals(CANDLE_COLOUR.RED)    && COLOUR_B.Equals(CANDLE_COLOUR.YELLOW) ||
                         currentColour.Equals(CANDLE_COLOUR.YELLOW) && COLOUR_B.Equals(CANDLE_COLOUR.RED))
                    currentColour = CANDLE_COLOUR.ORANGE;
                else if (currentColour.Equals(CANDLE_COLOUR.BLUE) && COLOUR_B.Equals(CANDLE_COLOUR.YELLOW) ||
                         currentColour.Equals(CANDLE_COLOUR.YELLOW) && COLOUR_B.Equals(CANDLE_COLOUR.BLUE))
                    currentColour = CANDLE_COLOUR.GREEN;
                else
                    currentColour = CANDLE_COLOUR.UNLIT;
            }
        }

        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        Color c;
        Gradient g;

        if (enumToColour.TryGetValue(currentColour, out c))
        {
            candleParticleSystem.gameObject.SetActive(true);
            candleLight.intensity = candleLightIntensity;
            candleLight.color = c;

            enumToGradient.TryGetValue(currentColour, out g);
            var col = candleParticleSystem.colorOverLifetime;
            col.color = g;
        }
        else
        {
            candleParticleSystem.gameObject.SetActive(false);
            candleLight.intensity = 0;
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
