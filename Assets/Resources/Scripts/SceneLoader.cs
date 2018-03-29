using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Interactable
{
    [SerializeField] private string nextScene;

    public override bool CanInteract()
    {
        return true;
    }

    public override void Interact()
    {
        PauseAndDeathManager.Instance().LoadScene(nextScene);
    }
}

