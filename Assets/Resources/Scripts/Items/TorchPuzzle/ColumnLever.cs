using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnLever : Interactable
{
    [SerializeField] private TorchColumn column;

    public override void Interact()
    {
        column.clockwise = !column.clockwise;
    }
}
