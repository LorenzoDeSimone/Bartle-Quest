using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateLevelPopup : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] string nextScene;
    private int position = -1;
    private float rateValue = 0f;

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("X"))
        {
            if (position >= 0)
                stars[position].color = new Color(stars[position].color.r, stars[position].color.g, stars[position].color.b, 0f);

            position--;
            rateValue -= 0.1f;
            if (position < 0)
            {
                rateValue = 0f;
                position = -1;
            }
        }
        else if (Input.GetButtonDown("B"))
        {
            if (position < 0)
            {
                stars[0].color = new Color(stars[0].color.r, stars[0].color.g, stars[0].color.b, 1f);
                position = 0;
                rateValue = 0.1f;
            }
            else
            {
                position++;
                rateValue += 0.1f;
                if (position >= stars.Length)
                {
                    rateValue = 1f;
                    position = stars.Length - 1;
                }
                stars[position].color = new Color(stars[position].color.r, stars[position].color.g, stars[position].color.b, 1f);
            }
        }
        else if(Input.GetButtonDown("A"))
        {
            Time.timeScale = 1f;
            PauseAndDeathManager.Instance().LoadScene(nextScene);
        }
    }
}
