using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHeadActivator : MonoBehaviour
{
    [SerializeField] GhostHead[] ghostHeads;

    public void ActivateGhostHead()
    {
        foreach (GhostHead head in ghostHeads)
            head.TryToActivate();
    }
}
