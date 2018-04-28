using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private Material ghostMaterial;

	// Use this for initialization
	void Start ()
    {
		if(PlayerChoices.Instance().CanSeeHiddenWalls)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().material = ghostMaterial;
        }
	}

}
