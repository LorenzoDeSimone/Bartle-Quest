using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDoor : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private RateLevelPopupLoader rateLevelPopupLoader;

    public void Explode()
    {
        if (rateLevelPopupLoader)
            rateLevelPopupLoader.gameObject.SetActive(true);

        if (GetComponent<Target>())
            GetComponent<Target>().enabled = false;

        explosion.transform.parent = null;
        explosion.gameObject.SetActive(true);
        explosion.Play();
        gameObject.SetActive(false);
    }
}
