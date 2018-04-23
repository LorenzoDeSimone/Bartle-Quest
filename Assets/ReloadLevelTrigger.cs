using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadLevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PlayerController>())
            PauseAndDeathManager.Instance().ReloadScene();
    }
}
