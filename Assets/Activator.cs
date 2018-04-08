using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public void ActiveStatus(bool value)
    {
        gameObject.SetActive(value);
    }
}
