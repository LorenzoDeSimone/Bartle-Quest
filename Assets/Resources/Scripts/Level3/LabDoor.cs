using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabDoor : MonoBehaviour
{
    [SerializeField] private ColouredProp heart;
    private PlayerCandle.CANDLE_COLOUR heartColour;

    [SerializeField] private ColouredProp stars;
    private PlayerCandle.CANDLE_COLOUR starsColour;

    [SerializeField] private ColouredProp crown;
    private PlayerCandle.CANDLE_COLOUR crownColour;

    public Material redMaterial, yellowMaterial, blueMaterial, orangeMaterial, greenMaterial, purpleMaterial;
    private Dictionary<PlayerCandle.CANDLE_COLOUR, Material> enumToMaterial;

    [SerializeField] private PlayerCandle playerCandle;


    void Start()
    {
        enumToMaterial = new Dictionary<PlayerCandle.CANDLE_COLOUR, Material>();
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.BLUE  , blueMaterial);
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.YELLOW, yellowMaterial);
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.RED   , redMaterial);
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.ORANGE, orangeMaterial);
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.GREEN , greenMaterial);
        enumToMaterial.Add(PlayerCandle.CANDLE_COLOUR.PURPLE, purpleMaterial);
    }

    public void ChangeMaterial(string ornamentName)
    {
        ColouredProp ornament = GetOrnamentFromName(ornamentName);
        MeshRenderer[] meshRenderers = ornament.GetComponentsInChildren<MeshRenderer>();
        Material m;
        if (enumToMaterial.TryGetValue(playerCandle.CurrentColour, out m))
        {
            foreach (MeshRenderer r in meshRenderers)
                r.material = m;

            ornament.currentColour = playerCandle.CurrentColour;
            if (IsCombinationRight())
                GetComponent<ExplodingDoor>().Explode();
        }
    }

    private bool IsCombinationRight()
    {
        //I know...
        return heart.currentColour.Equals(PlayerCandle.CANDLE_COLOUR.YELLOW) &&
               stars.currentColour.Equals(PlayerCandle.CANDLE_COLOUR.PURPLE) &&
               crown.currentColour.Equals(PlayerCandle.CANDLE_COLOUR.ORANGE);
    }

    private ColouredProp GetOrnamentFromName(string name)
    {
        if (name.Equals("heart"))
            return heart;
        else if (name.Equals("stars"))
            return stars;
        else if (name.Equals("crown"))
            return crown;
        else
            return null;
    }
}
