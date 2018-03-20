using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLever : Interactable
{
    [SerializeField] private TorchColumn[] columns;

    public override void Interact()
    {
        foreach(TorchColumn t in columns)
        {
            t.Rotate();
        }
    }
}
