using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChanger : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private float colourChangeTime;


    void OnTriggerEnter(Collider collider)
    {
        if(collider.GetComponent<PlayerController>())
            StartCoroutine(ChangeGlobalLightColour());
    }

    private IEnumerator ChangeGlobalLightColour()
    {
        float elapsedTime = 0f;
        Color startColour = RenderSettings.ambientLight;

        while (elapsedTime < colourChangeTime)
        {
            elapsedTime += Time.deltaTime;
            RenderSettings.ambientLight = Color.Lerp(startColour, color, elapsedTime / colourChangeTime);
            yield return null;
        }
        RenderSettings.ambientLight = color;
    }
}
