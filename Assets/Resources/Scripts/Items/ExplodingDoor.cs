using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingDoor : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private RateLevelPopupLoader rateLevelPopupLoader;
    [SerializeField] private bool successSound = true;

    public void Explode()
    {
        if (rateLevelPopupLoader)
            rateLevelPopupLoader.gameObject.SetActive(true);

        if (GetComponent<Target>())
            GetComponent<Target>().enabled = false;

        if (successSound)
            AudioManager.Instance().PlaySuccess();

        explosion.transform.parent = null;
        explosion.gameObject.SetActive(true);
        explosion.Play();
       

        gameObject.SetActive(false);
    }
}
