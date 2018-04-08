using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNotifier : MonoBehaviour
{
    [SerializeField] private ParticleSystem lordFloatingSkullParticle;

    public void SetFloatingSkull(ParticleSystem floatingSkull)
    {
        lordFloatingSkullParticle = floatingSkull;
    }

    void OnDestroy()
    {
        lordFloatingSkullParticle.gameObject.SetActive(false);
    }
}
