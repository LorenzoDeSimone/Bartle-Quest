using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private Hittable playerHittable;
    [SerializeField] private Image[] UIHearts;
    [SerializeField] private Color Full;
    [SerializeField] private Color Empty;

    private enum HearthState { FULL, EMPTY, INVISIBLE };

    // Update is called once per frame
    void Update ()
    {
        for (int i = 0; i < UIHearts.Length; i++)
        {
            if(i < playerHittable.CurrentHealth)
                SetHearthState(UIHearts[i], HearthState.FULL);
            else if (i < playerHittable.MaxHealth)
                SetHearthState(UIHearts[i], HearthState.EMPTY);
            else
                SetHearthState(UIHearts[i], HearthState.INVISIBLE);
        }
    }
    
    private void SetHearthState(Image heart, HearthState state)
    {
        if(state.Equals(HearthState.FULL))
        {
            heart.enabled = true;
            heart.color = Full;
        }
        else if (state.Equals(HearthState.EMPTY))
        {
            heart.enabled = true;
            heart.color = Empty;
        }
        else
            heart.enabled = false;
    }
}
