using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDoor : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    
    public void Explode()
    {
        explosion.transform.parent = null;
        explosion.gameObject.SetActive(true);
        explosion.Play();
        gameObject.SetActive(false);

        if(GetComponent<Target>())
            GetComponent<Target>().enabled = false;
    }
}
