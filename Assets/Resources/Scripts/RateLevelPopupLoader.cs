using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RateLevelPopupLoader : Interactable
{
    [SerializeField] RateLevelPopup rateLevelPopup;

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        Time.timeScale = 0f;
        rateLevelPopup.gameObject.SetActive(true);
    }
}

